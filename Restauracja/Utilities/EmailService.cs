using HtmlAgilityPack;
using Restauracja.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restauracja.Utilities
{
    public class EmailService
    {
        public const string EMAIL_SUBJECT = "Nowe zamówienie klienta";
        public const string GMAIL_SMTP_HOST = "smtp.gmail.com";
        public const int GMAIL_SMTP_PORT = 587;

        public Dictionary<string, string> ComposingParts { get; set; } = new Dictionary<string, string>();

        public string Sender { get; set; }
        public string Recipent { get; set; }
        public OrderPOCO Order { get; set; }


        public EmailService(string sender, string recipent, OrderPOCO order)
        {
            Sender = sender;
            Recipent = recipent;
            Order = order;
        }

        public string GenerateEmail()
        {
            var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(q => q.Contains("ristorante_footer_pattern.html")).FirstOrDefault();
            var currency = CultureInfo.GetCultureInfo("pl-PL").NumberFormat.CurrencySymbol;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string html = reader.ReadToEnd();
                    var addedProductsHtml = GenerateContent(html);

                    StringBuilder builder = new StringBuilder();
                    var newHtml = addedProductsHtml
                        .Replace("$$CLIENT_EMAIL$$", Sender)
                        .Replace("$$DESCRIPTION$$", Order.Description)
                        .Replace("$$FINAL_COST$$", builder.Clear().Append(Order.FinalCost.ToString()).Append(" ").Append(currency).ToString())
                        .Replace("$$SENTFROM@MAIL$$",
                        Sender);

                    doc.LoadHtml(newHtml);
                    var replacedHtml = ReplaceNodeAttribute(doc, "href", "senderEmail", "a", "mailto:" + Sender);

                    //File.WriteAllText(@"ristorante_footer_to_sent.html", html, Encoding.UTF8); //save (cache email to be sent)
                    return replacedHtml;
                }
            }
        }

        private string ReplaceNodeAttribute(HtmlAgilityPack.HtmlDocument doc, string attribute, string withId, string htmlTag, string attValue)
        {
            var query = $"//{htmlTag}[@id='{withId}']";
            HtmlNode node = doc.DocumentNode.SelectSingleNode(query);
            node.SetAttributeValue(attribute, attValue);
            var newHtml = doc.DocumentNode.WriteTo();
            return newHtml;
        }

        private string GenerateContent(string html)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(html);
            HtmlNode tableNode = htmlDoc.DocumentNode.SelectSingleNode($"//table[@id='contentsTable']");

            var allTrNodes = tableNode.SelectNodes($@"//tr[@id='contentsRow']");
            // two first rows
            var priorNodes = new List<HtmlNode>();
            priorNodes.Add(allTrNodes[0]);
            priorNodes.Add(allTrNodes[1]);

            var restOfNodes = allTrNodes.Except(priorNodes);

            tableNode.RemoveAllChildren();
            tableNode.AppendChild(allTrNodes[0]);
            tableNode.AppendChild(allTrNodes[1]);

            // before appending products nodes we need to copy all the nodes except for the first two and append them to the end
            foreach (var prod in Order.Products)
            {
                var productNode = $@"<tr><td><p style='text-decoration: none; color: #5a5d64;
                                            margin-bottom: $$MARGIN_BOTTOM$$px;
                                            padding-top: $$PADDING_TOP$$px;'>$$PRODUCT_DATA$$</p>
                                    </td>
                                </tr>";

                var styledNode = Style(productNode, prod);
                var newNode = InsertDataTo(styledNode, prod);

                tableNode.AppendChild(HtmlNode.CreateNode(newNode));
            }

            foreach (var node in restOfNodes)
            {
                tableNode.AppendChild(node);
            }
            return htmlDoc.DocumentNode.InnerHtml;
        }

        private string Style(string productNode, ProductPOCO currProd)
        {
            var paddingVal = 0;
            var marginVal = 3;

            if (Order.Products.IndexOf(currProd) == 0)
            {
                paddingVal = 25;
            }
            if (Order.Products.IndexOf(currProd) == Order.Products.Count - 1)
            {
                marginVal = 15;
            }

            var newOne = productNode.Replace("$$MARGIN_BOTTOM$$", marginVal.ToString()).Replace("$$PADDING_TOP$$", paddingVal.ToString());
            return newOne;
        }

        private string InsertDataTo(string productNode, ProductPOCO prod)
        {
            var currency = CultureInfo.GetCultureInfo("pl-PL").NumberFormat.CurrencySymbol;

            StringBuilder sb = new StringBuilder();
            var productData = sb.Append(Order.Products[Order.Products.IndexOf(prod)].Name)
                                               .Append(" ")
                                               .Append(Order.Products[Order.Products.IndexOf(prod)].Price)
                                               .Append(" ").Append(currency).Append(" x ")
                                               .Append(Order.Products[Order.Products.IndexOf(prod)].Quantity)
                                               .Append(" = ")
                                               .Append(Order.Products[Order.Products.IndexOf(prod)].Price * Order.Products[Order.Products.IndexOf(prod)].Quantity)
                                               .Append(" ")
                                               .Append(currency).ToString();

            var newProductNode = productNode.Replace("$$PRODUCT_DATA$$", productData);
            return newProductNode;
        }
    }

}
