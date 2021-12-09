using Microsoft.Web.WebView2.WinForms;
using PowerTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsUIWebAPI.Extensions
{
    public static class WebViewExtentions
    {

        public static async Task<Product> CreateBandQProduct(this WebView2 webView2, List<Product> products)
        {
            // product code
            var productCodeResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('d13cb219 _58c871b0 b1bfb616')[document.getElementsByClassName('d13cb219 _58c871b0 b1bfb616').length -1].innerHTML");
            var productCode = productCodeResult.ClearFormat();

            var searchProduct = ProductExists(productCode, products);
            if (searchProduct != null)
            {
                MessageBox.Show("this Product is in the Database");
                return searchProduct;
            }
            // get productname
            string productNameResult = await webView2.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName('ccb9d67a _17d3fa36 _1c13b5e2 _58b3d2d9 _514c3e90 _75d33510 _266816c0 _6ba14bc3 fcf8ebfc _78852320 ee063513 cc6bbaee _23ee746f')[0].innerText");
            var productName = productNameResult.ClearFormat();

            // getImage
            string firstImgResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('_2fc6e0e9 b4d87c1d _78f9416e ce5065a9')[0].src");
            var imageUrl = firstImgResult.ClearFormat();

            var priceResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('b25ad5d5 _4e80f7be _23ee746f _7b343263 _21dc035c')[0].getElementsByTagName('span')[1].innerText");
            var unitPrice = priceResult.ClearFormat();

            var description = await webView2.ExecuteScriptAsync("document.getElementsByClassName('_7be6e5a0 _66dabd6a e0657c31')[0].innerText;");

            var product = new Product();
            product.ProductCode = productCode;
            product.ProductName = productName;
            product.ImageUrl = imageUrl;
            product.Price = decimal.Parse(unitPrice);
            product.SupplierName = "B&Q";
            product.SupplierIcon = "bq.png";
            product.ContentDetails = description.ClearFormatNewLine();
            product.IsUptodate = true;
            if (productName.Length > 35)
            {
                product.Search = productName.Substring(0, 35);
            }
            else
            {
                product.Search = productName;
            }
            return await Task.FromResult(product);
        }

        public static async Task<Product> CreatepowertoolworldProduct(this WebView2 webView2, List<Product> products)
        {


            var skuResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName(' col-xs-12 push-half--bottom product-sku-ean')[0].getElementsByTagName('span')[0].innerText;");

            var resultLastIndex = skuResult.LastIndexOf(" ");
            var productCode = skuResult.Substring(resultLastIndex + 1, skuResult.Length - (resultLastIndex + 1)).ClearFormat();

            var searchProduct = ProductExists(productCode, products);
            if (searchProduct != null)
            {
                MessageBox.Show("this Product is in the Database");
                return searchProduct;
            }

            // get productname
            string productNameResult = await webView2.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName('base')[0].innerHTML");
            var productName = productNameResult.ClearFormat().Trim();

            // getImage
            string firstImgResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('MagicZoom')[0].href");
            string imageUrl = "";
            if (firstImgResult != null)
            {

                imageUrl = firstImgResult.ClearFormat();
            }

            var priceResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('price')[1].innerText");
            var unitPrice = priceResult.ClearFormat();

            var descriptionResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('product-description col-xs-12 col-md-9 start-xs value')[0].innerText");
            var description = descriptionResult.ClearFormatNewLine();


            var product = new Product();
            product.ProductCode = productCode;
            product.ProductName = productName;
            product.ImageUrl = imageUrl;
            product.Price = decimal.Parse(unitPrice);
            product.SupplierName = "Power Toolworld";
            product.SupplierIcon = "pwt.png";
            product.ContentDetails = description.ClearFormatNewLine();
            product.IsUptodate = true;
            if (productName.Length > 35)
            {
                product.Search = productName.Substring(0, 35);
            }
            else
            {
                product.Search = productName;
            }



            return await Task.FromResult(product);
        }

        public static async Task<Product> CreateScrewfixProduct(this WebView2 webView2, List<Product> products)
        {

            var productCodeResult = await webView2.ExecuteScriptAsync("document.getElementById('product_code_container').getElementsByTagName('span')[0].innerText");

            var productCode = productCodeResult.ClearFormat();

            var searchProduct = ProductExists(productCode, products);
            if (searchProduct != null)
            {
                MessageBox.Show("this Product is in the Database");
                return searchProduct;
            }
            string productNameResult = await webView2.ExecuteScriptAsync("document.getElementById('product_description').getElementsByTagName('span')[0].innerText");
            var productName = productNameResult.ClearFormat();



            string imgResult = await webView2.ExecuteScriptAsync("document.getElementById('product_image_0').src");
            var imageUrl = imgResult.ClearFormat();

            var priceResult = await webView2.ExecuteScriptAsync("document.getElementById('product_price').innerText");
            var unitPrice = priceResult.ClearFormat();


            var description = await webView2.ExecuteScriptAsync("document.getElementById('product_long_description_container').innerText");
            //var descriptionresult = await webView2.ExecuteScriptAsync("document.getElementById('product_specification_list').innerText");

            var product = new Product();
            product.ProductCode = productCode;
            product.ProductName = productName;
            product.ImageUrl = imageUrl;
            product.Price = decimal.Parse(unitPrice);
            product.SupplierName = "Screwfix";
            product.SupplierIcon = "sf.png";
            product.ContentDetails = description.ClearFormatNewLine();
            product.IsUptodate = true;
            if (productName.Length > 35)
            {
                product.Search = productName.Substring(0, 35);
            }
            else
            {
                product.Search = productName;
            }
            return await Task.FromResult(product); ;

        }
        public static async Task<Product> CreatepowerWickesProduct(this WebView2 webView2, List<Product> products)
        {


            var productCodeResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('product-code product-code-v2')[0].innerText");
            var productCode = productCodeResult.ClearFormat().TrimStart();

            var searchProduct = ProductExists(productCode, products);

            if (searchProduct != null)
            {
                MessageBox.Show("this Product is in the Database");
                return searchProduct;
            }
            string productNameResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('pdp__heading')[0].innerText");
            var productName = productNameResult.Replace("\"", "");

            string imgResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('s7staticimage')[0].getElementsByTagName('img')[0].src;");
            var imageUrl = imgResult.ClearFormat();

            var priceResult = await webView2.ExecuteScriptAsync("document.getElementsByClassName('pdp-price__new-price')[0].innerText");
            var unitPrice = priceResult.ClearFormat();

            var description = await webView2.ExecuteScriptAsync("document.getElementsByClassName('product-main-info__description')[0].innerText");



            var product = new Product();
            product.ProductCode = productCode;
            product.ProductName = productName;
            product.ImageUrl = imageUrl;
            product.Price = decimal.Parse(unitPrice);
            product.SupplierName = "Wickes";
            product.SupplierIcon = "wks.png";
            product.ContentDetails = description.ClearFormatNewLine();
            product.IsUptodate = true;
            if (productName.Length > 35)
            {
                product.Search = productName.Substring(0, 35);
            }
            else
            {
                product.Search = productName;
            }
            return await Task.FromResult(product);

        }
        private static Product ProductExists(string productCode, List<Product> products)
        {
            var result = products.FirstOrDefault(p => p.ProductCode == productCode);
            return result;
        }

        public static async void SearchSite(this WebView2 webView2, string formScript, string submitScript)
        {

            await webView2.ExecuteScriptAsync(formScript);
            await webView2.ExecuteScriptAsync(submitScript);
        }
        public static string ClearFormat(this string stringFormat)
        {
            return stringFormat.Replace("\\t", " ").Replace("\\n", "").Replace("£", "").Replace("\"", "").Replace("INC VAT", "").Replace("Product code:", "");
        }
        public static string ClearFormatNewLine(this string stringFormat)
        {
            return stringFormat.Replace("\\t", " ").Replace("\\n", Environment.NewLine).Replace("£", "").Replace("\"", "");
        }
    }
}
