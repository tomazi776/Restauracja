using HtmlAgilityPack;
using Restauracja.Model;
using Restauracja.Utilities;
using Restauracja.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security;
using System.Text;
using System.Windows;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Restauracja.Services
{
    public class EmailService
    {
        private const string EMAIL_SUBJECT = "Nowe zamówienie klienta";
        private const string GMAIL_SMTP_HOST = "smtp.gmail.com";
        public const int GMAIL_SMTP_PORT = 587;

        private string Sender { get; set; }
        private OrderPOCO Order { get; set; }
        private string Currency { get; set; } = CultureInfo.GetCultureInfo("pl-PL").NumberFormat.CurrencySymbol;


        public EmailService(string sender, OrderPOCO order)
        {
            Sender = sender;
            Order = order;
        }

        public string GenerateEmail()
        {
            var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(q => q.Contains("ristorante_footer_pattern.html")).FirstOrDefault();
            HtmlDocument doc = new HtmlDocument();

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string html = reader.ReadToEnd();
                    doc.LoadHtml(html);
                    var addedProductsHtml = GenerateHtml(doc);

                    doc.LoadHtml(addedProductsHtml);
                    var replacedHtml = ReplaceNodeAttribute(doc, "href", "senderEmail", "a", "mailto:" + Sender);
                    File.WriteAllText(@"ristorante_footer_to_sent.html", replacedHtml, Encoding.UTF8); //save (cache email to be sent)
                    return replacedHtml;
                }
            }
        }

        public void SendEmail(OrderSummaryViewModel orderSummaryVm, string emailBodyInHtml, SecureString pass)
        {
            MailMessage email = new MailMessage(orderSummaryVm.Sender, orderSummaryVm.Recipent);
            email.IsBodyHtml = true;
            email.Body = emailBodyInHtml;
            email.Subject = EMAIL_SUBJECT;

            SmtpClient smtpClient = new SmtpClient(GMAIL_SMTP_HOST, GMAIL_SMTP_PORT);
            if (orderSummaryVm.Sender != null && pass != null)
            {
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(orderSummaryVm.Sender, pass);
            }
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 10000;
            smtpClient.Send(email);          
        }

        private string ReplaceNodeAttribute(HtmlDocument doc, string attribute, string withId, string htmlTag, string attValue)
        {
            var query = $"//{htmlTag}[@id='{withId}']";
            HtmlNode node = doc.DocumentNode.SelectSingleNode(query);
            node.SetAttributeValue(attribute, attValue);
            var newHtml = doc.DocumentNode.WriteTo();
            return newHtml;
        }

        private string GenerateHtml(HtmlDocument doc)
        {
            HtmlNode tableNode = doc.DocumentNode.SelectSingleNode($"//table[@id='contentsTable']");
            var tableTrNodes = tableNode.SelectNodes($@"//tr[@id='contentsRow']");

            var priorNodes = new List<HtmlNode>() { tableTrNodes[0], tableTrNodes[1] };
            var remaining = tableTrNodes.Except(priorNodes);

            tableNode.RemoveAllChildren();
            tableNode.AppendChild(tableTrNodes[0]);
            tableNode.AppendChild(tableTrNodes[1]);

            AppendGeneratedRowsTo(tableNode);
            AppendRemainingRowsTo(tableNode, remaining);
            return ReplaceStaticHtmlWithOrderData(doc);
        }

        private string ReplaceStaticHtmlWithOrderData(HtmlDocument doc)
        {
            StringBuilder builder = new StringBuilder();
            return doc.DocumentNode.InnerHtml
                .Replace("$$CLIENT_EMAIL$$", Sender)
                .Replace("$$DESCRIPTION$$", Order.Description)
                .Replace("$$FINAL_COST$$", builder.Append(Order.FinalCost.ToString()).Append(" ").Append(Currency).ToString())
                .Replace("$$SENTFROM@MAIL$$", Sender);
        }

        private void AppendRemainingRowsTo(HtmlNode tableNode, IEnumerable<HtmlNode> remaining)
        {
            foreach (var node in remaining)
            {
                tableNode.AppendChild(node);
            }
        }

        private void AppendGeneratedRowsTo(HtmlNode tableNode)
        {
            foreach (var prod in Order.Products)
            {
                var tableRow = GenerateTableRowWithProductData(prod);
                tableNode.AppendChild(HtmlNode.CreateNode(tableRow));
            }
        }

        private string GenerateTableRowWithProductData(ProductPOCO prod)
        {
            var productNode = $@"<tr>
                                    <td>
                                        <p style='text-decoration: none; color: #5a5d64;
                                            margin-bottom: $$MARGIN_BOTTOM$$px;
                                            padding-top: $$PADDING_TOP$$px;'>$$PRODUCT_DATA$$</p>
                                    </td>
                                </tr>";

            var styledNode = Style(productNode, prod);
            var newNode = InsertDataTo(styledNode, prod);
            return newNode;
        }

        private string Style(string productNode, ProductPOCO currProd)
        {
            var paddingVal = 0;
            var marginVal = 3;
            if (Order.Products.IndexOf(currProd) == 0)
                paddingVal = 25;
            if (Order.Products.IndexOf(currProd) == Order.Products.Count - 1)
                marginVal = 15;
            return productNode
                .Replace("$$MARGIN_BOTTOM$$", marginVal.ToString())
                .Replace("$$PADDING_TOP$$", paddingVal.ToString());
        }

        private string InsertDataTo(string productNode, ProductPOCO prod)
        {
            StringBuilder sb = new StringBuilder();
            var productData = sb
                .Append(Order.Products[Order.Products.IndexOf(prod)].Name)
                .Append(" ")
                .Append(Order.Products[Order.Products.IndexOf(prod)].Price)
                .Append(" ").Append(Currency).Append(" x ")
                .Append(Order.Products[Order.Products.IndexOf(prod)].Quantity)
                .Append(" = ")
                .Append(Order.Products[Order.Products.IndexOf(prod)].Price * Order.Products[Order.Products.IndexOf(prod)].Quantity)
                .Append(" ")
                .Append(Currency).ToString();
            return productNode.Replace("$$PRODUCT_DATA$$", productData);
        }
    }
}
