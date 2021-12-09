using Microsoft.Web.WebView2.Core;
using PowerTools.Models;
using System.ComponentModel;
using WebAPI.ProductClient;
using WinFormsUIWebAPI.Extensions;

namespace WinFormsUIWebAPI
{
    public partial class FormProducts : Form
    {
        private ClientProducts _client;
        private List<Product> _products;
        public FormProducts()
        {
            InitializeComponent();
            webViewBandQ.Source = new Uri(@"https://www.diy.com/");
            webViewPowertoolworld.Source = new Uri(@"https://www.powertoolworld.co.uk/");
            webViewWickes.Source = new Uri(@"https://www.wickes.co.uk/");
            webViewScrewfix.Source = new Uri(@"https://www.screwfix.com/");
            googleWebView.Source = new Uri(@"https://www.google.co.uk/");
            _client = new ClientProducts("https://localhost:7196/", new HttpClient());
            Initialize();
        }
        async void Initialize()
        {
            const string postMessage = "window.chrome.webview.postMessage(window.document.URL);";
            await webViewBandQ.EnsureCoreWebView2Async(null);
            webViewBandQ.CoreWebView2.WebMessageReceived += UpdateBQAddressBar;
            await webViewBandQ.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(postMessage);


            await webViewPowertoolworld.EnsureCoreWebView2Async(null);
            webViewPowertoolworld.CoreWebView2.WebMessageReceived += UpdateAddressBar;
            await webViewPowertoolworld.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(postMessage);

            await webViewWickes.EnsureCoreWebView2Async(null);
            webViewWickes.CoreWebView2.WebMessageReceived += UpdatewebViewWickesAddressBar;
            await webViewWickes.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(postMessage);

            await webViewScrewfix.EnsureCoreWebView2Async(null);
            webViewScrewfix.CoreWebView2.WebMessageReceived += UpdatewebViewScrewfixAddressBar;
            await webViewScrewfix.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(postMessage);

            await googleWebView.EnsureCoreWebView2Async(null);
            googleWebView.CoreWebView2.WebMessageReceived += UpdatewebViewGoogleAddressBar;
            await googleWebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(postMessage);
            // await _viewModel.GetProducts();

            var clientProduct = await _client.GetProductsAsync();
            _products = clientProduct.ToList();
            productBindingSource.DataSource = new BindingList<Product>(_products);
        }
        private void dataGridViewProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataGridViewProducts.Columns[e.ColumnIndex].Name == "IsUptodateDataGridImageColumn")
                {
                    Bitmap imageToDraw;
                    var isUptodate = (bool)dataGridViewProducts["IsUptodateDataGridImageColumn", e.RowIndex].Value;
                    try
                    {
                        if (isUptodate)
                        {
                            imageToDraw = Properties.Resources.pin_green;
                            imageToDraw.MakeTransparent(Color.FromArgb(238, 238, 238));
                        }
                        else
                        {
                            imageToDraw = Properties.Resources.pin_red;
                            imageToDraw.MakeTransparent(Color.FromArgb(238, 238, 238));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("The_image_file" + ex.ToString());
                        return;

                    }
                    e.Value = imageToDraw;
                    e.FormattingApplied = true;
                }
                if (dataGridViewProducts.Columns[e.ColumnIndex].Name == "ColumnSupplierIcon")
                {
                    Bitmap imageToDraw;
                    var imageName = (string)dataGridViewProducts["ColumnSupplierIcon", e.RowIndex].Value;
                    if (imageName == "bq.png")
                    {
                        imageToDraw = Properties.Resources.bq;
                        imageToDraw.MakeTransparent(Color.FromArgb(238, 238, 238));
                    }
                    else if (imageName == "pwt.png")
                    {
                        imageToDraw = Properties.Resources.pwt;
                        imageToDraw.MakeTransparent(Color.FromArgb(238, 238, 238));
                    }
                    else if (imageName == "wks.png")
                    {
                        imageToDraw = Properties.Resources.wks;
                        imageToDraw.MakeTransparent(Color.FromArgb(238, 238, 238));
                    }
                    else if (imageName == "sf.png")
                    {
                        imageToDraw = Properties.Resources.sf;
                        imageToDraw.MakeTransparent(Color.FromArgb(238, 238, 238));
                    }
                    else
                    {
                        imageToDraw = Properties.Resources.pin_red;
                        imageToDraw.MakeTransparent(Color.FromArgb(238, 238, 238));
                    }
                    e.Value = imageToDraw;
                    e.FormattingApplied = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Update Address Bar
        private void UpdatewebViewGoogleAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string uri = args.TryGetWebMessageAsString();
            GoogleToolStripTextBox.Text = uri;
            googleWebView.CoreWebView2.PostWebMessageAsString(uri);
        }
        private void UpdatewebViewScrewfixAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string uri = args.TryGetWebMessageAsString();
            sfUrlToolStripTextBox.Text = uri;
            webViewScrewfix.CoreWebView2.PostWebMessageAsString(uri);
        }
        private void UpdatewebViewWickesAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string uri = args.TryGetWebMessageAsString();
            wicSearchToolStripTextBox.Text = uri;
            webViewWickes.CoreWebView2.PostWebMessageAsString(uri);
        }
        private void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string uri = e.TryGetWebMessageAsString();
            PowertoolworldToolStripTextBox.Text = uri; //uri;
            webViewBandQ.CoreWebView2.PostWebMessageAsString(uri);
        }
        private void UpdateBQAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string uri = e.TryGetWebMessageAsString();
            toolStripTextBoxBandQ.Text = webViewBandQ.Source.ToString(); //uri;
            webViewBandQ.CoreWebView2.PostWebMessageAsString(uri);
        }
        #endregion

        #region Powertoolworld
        private async void ptSaveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var product = await webViewPowertoolworld.CreatepowertoolworldProduct(_products);
                var formProduct = new FormProduct(product);
                if (formProduct.ShowDialog() == DialogResult.OK)
                {
                    var newProduct = await _client.PostProductAsync(product);
                    product.Id = newProduct.Id;
                    _products?.Add(product);
                    productBindingSource.DataSource = new BindingList<Product>(_products); ;
                    productBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void ptBacktoolStripButton_Click(object sender, EventArgs e)
        {
            if (webViewPowertoolworld.CanGoBack)
                webViewPowertoolworld.GoBack();
        }
        private void ptForwordtoolStripButton_Click(object sender, EventArgs e)
        {
            if (webViewPowertoolworld.CanGoForward)
                webViewPowertoolworld.GoForward();
        }
        private void ptRefreshtoolStripButton_Click(object sender, EventArgs e)
        {
            webViewPowertoolworld.Reload();
        }
        private void ptCanceltoolStripButton4_Click(object sender, EventArgs e)
        {
            webViewPowertoolworld.Stop();
        }
        #endregion

        #region B&Q
        private async void saveBQToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var product = await webViewBandQ.CreateBandQProduct(_products);
                if (product.ProductName == "null") throw new Exception();
                var formProduct = new FormProduct(product);
                if (formProduct.ShowDialog() == DialogResult.OK)
                {
                    var newProduct = await _client.PostProductAsync(product);
                    product.Id = newProduct.Id;
                    _products.Add(product);
                    productBindingSource.DataSource = new BindingList<Product>(_products); ;
                    productBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void bandQBackToolStripButton_Click(object sender, EventArgs e)
        {
            if (webViewBandQ.CanGoBack)
                webViewBandQ.GoBack();
        }

        private void bandqForwardToolStripButton_Click(object sender, EventArgs e)
        {
            if (webViewBandQ.CanGoForward)
                webViewBandQ.GoForward();
        }

        private void bandqRefreshToolStripButton_Click(object sender, EventArgs e)
        {
            webViewBandQ.Reload();
        }

        private void bandqStopToolStripButton_Click(object sender, EventArgs e)
        {
            webViewBandQ.Stop();
        }


        #endregion

        #region Wickes
        private async void wicSavetoolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var product = await webViewWickes.CreatepowerWickesProduct(_products);
                var formProduct = new FormProduct(product);
                if (formProduct.ShowDialog() == DialogResult.OK)
                {
                    var newProduct = await _client.PostProductAsync(product);
                    product.Id = newProduct.Id;
                    _products?.Add(product);
                    productBindingSource.DataSource = new BindingList<Product>(_products); ;
                    productBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void wicBackToolStripButton_Click(object sender, EventArgs e)
        {
            // if (webViewPowertoolworld.CanGoBack)
            if (webViewWickes.CanGoBack)
                webViewWickes.GoBack();
        }
        private void wicForwardToolStripButton_Click(object sender, EventArgs e)
        {
            if (webViewWickes.CanGoForward)
                webViewWickes.GoForward();
        }
        private void wicRefreshToolStripButton_Click(object sender, EventArgs e)
        {
            webViewWickes.Reload();
        }
        private void wicCancelToolStripButton_Click(object sender, EventArgs e)
        {
            webViewWickes.Stop();
        }
        #endregion

        #region Context Menu
        private void showPowerToolsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bqList = new BindingList<Product>(_products.Where(p => p.SupplierName == "Power Toolworld").ToList());
            productBindingSource.DataSource = bqList;
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedProduct = (Product)productBindingSource.Current;

            var toolstationSearchScript = $"document.forms[0].elements[0].value = '{selectedProduct.Search?.ToLower()}';";
            var toolstationSubmitScript = "document.forms[0].submit();";
            webViewPowertoolworld.SearchSite(toolstationSearchScript, toolstationSubmitScript);

            webViewWickes.SearchSite($"document.getElementById('search').value = '{selectedProduct.Search?.ToLower()}';", "document.forms[0].submit();");
            webViewBandQ.SearchSite($"document.forms[0].elements[0].value = '{selectedProduct.Search?.ToLower()}';", "document.forms[0].submit();");
            webViewScrewfix.SearchSite($"document.getElementById('mainSearch-input').value = '{selectedProduct.Search?.ToLower()}';", "document.getElementById('search_submit_button').click();");
            googleWebView.SearchSite($"document.forms[0].elements[0].value = '{selectedProduct.Search?.ToLower()}';", "document.forms[0].submit();");

        }
        private void serchByProductNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedProduct = (Product)productBindingSource.Current;

            webViewPowertoolworld.SearchSite($"document.forms[0].elements[0].value = '{selectedProduct.ProductName?.ToLower()}';", "document.forms[0].submit();");
            webViewWickes.SearchSite($"document.getElementById('search').value = '{selectedProduct.ProductName?.ToLower()}';", "document.forms[0].submit();");
            webViewBandQ.SearchSite($"document.forms[0].elements[0].value = '{selectedProduct.ProductName?.ToLower()}';", "document.forms[0].submit();");
            webViewScrewfix.SearchSite($"document.getElementById('mainSearch-input').value = '{selectedProduct.ProductName?.ToLower()}';", "document.getElementById('search_submit_button').click();");
            googleWebView.SearchSite($"document.forms[0].elements[0].value = '{selectedProduct.ProductName?.ToLower()}';", "document.forms[0].submit();");
        }

        private async void openProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (productBindingSource.Count > 0)
            {
                // _viewModel.Test();
                var productForEdit = (Product)productBindingSource.Current;
                var formProduct = new FormProduct(productForEdit);
                if (formProduct.ShowDialog() == DialogResult.OK)
                {
                    await _client.PutProductAsync(productForEdit.Id, productForEdit);
                    productBindingSource.EndEdit();
                }
            }
        }

        private void showBQOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bqList = new BindingList<Product>(_products.Where(p => p.SupplierName == "B&Q").ToList());
            productBindingSource.DataSource = bqList;
        }

        private void wickesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bqList = new BindingList<Product>(_products.Where(p => p.SupplierName == "Wickes").ToList());
            productBindingSource.DataSource = bqList;
        }

        private void screwfixListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bqList = new BindingList<Product>(_products.Where(p => p.SupplierName == "Screwfix").ToList());
            productBindingSource.DataSource = bqList;
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            productBindingSource.DataSource = new BindingList<Product>(_products);
        }



        #endregion

        #region Screwfix
        private async void sfSaveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var product = await webViewScrewfix.CreateScrewfixProduct(_products);
                var formProduct = new FormProduct(product);
                if (formProduct.ShowDialog() == DialogResult.OK)
                {
                    var newProduct = await _client.PostProductAsync(product);
                    product.Id = newProduct.Id;
                    _products?.Add(product);
                    productBindingSource.DataSource = new BindingList<Product>(_products); ;
                    productBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void sfBackToolStripButton_Click(object sender, EventArgs e)
        {
            if (webViewScrewfix.CanGoBack)
                webViewScrewfix.GoBack();
        }

        private void sfForwardToolStripButton_Click(object sender, EventArgs e)
        {
            if (webViewScrewfix.CanGoForward)
                webViewScrewfix.GoForward();
        }

        private void sfRefreshToolStripButton_Click(object sender, EventArgs e)
        {
            webViewScrewfix.Refresh();
        }

        private void sfCancelToolStripButton_Click(object sender, EventArgs e)
        {
            webViewScrewfix.Stop();
        }

        #endregion

        #region google
        private void goBackToolStripButton_Click(object sender, EventArgs e)
        {
            if (googleWebView.CanGoBack)
                googleWebView.GoBack();

        }

        private void goForwardToolStripButton_Click(object sender, EventArgs e)
        {
            if (googleWebView.CanGoForward)
                googleWebView.GoBack();

        }

        private void goRefreshToolStripButton_Click(object sender, EventArgs e)
        {
            googleWebView.Refresh();
        }

        private void goCancelToolStripButton_Click(object sender, EventArgs e)
        {
            googleWebView.Stop();
        }


        #endregion

        private void textBoxSearchProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var searchProduct = textBoxSearchProducts.Text;
                e.Handled = true;
                e.SuppressKeyPress = true;
                webViewPowertoolworld.SearchSite($"document.forms[0].elements[0].value = '{searchProduct.ToLower()}';", "document.forms[0].submit();");
                webViewWickes.SearchSite($"document.getElementById('search').value = '{searchProduct.ToLower()}';", "document.forms[0].submit();");
                webViewBandQ.SearchSite($"document.forms[0].elements[0].value = '{searchProduct.ToLower()}';", "document.forms[0].submit();");
                webViewScrewfix.SearchSite($"document.getElementById('mainSearch-input').value = '{searchProduct.ToLower()}';", "document.getElementById('search_submit_button').click();");
                googleWebView.SearchSite($"document.forms[0].elements[0].value = '{searchProduct.ToLower()}';", "document.forms[0].submit();");
                var searchList = new List<string>();
                if (!string.IsNullOrEmpty(searchProduct))
                {
                    searchList.AddRange(searchProduct.ToLower().Split(" ").ToList());
                    if (searchList.Count == 1)
                    {
                        var result = _products?.Where(p => p.ProductName.ToLower().Contains(searchList[0].ToLower())).ToList();
                        if (result?.Count > 0) productBindingSource.DataSource = new BindingList<Product>(result);
                    }
                    if (searchList.Count == 2)
                    {
                        var result = _products?.Where(
                            p => p.ProductName.ToLower().Contains(searchList[0].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[1].ToLower())).ToList();
                        if (result?.Count > 0) productBindingSource.DataSource = new BindingList<Product>(result);
                    }
                    if (searchList.Count == 3)
                    {
                        var result = _products?.Where(
                            p => p.ProductName.ToLower().Contains(searchList[0].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[1].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[2].ToLower())).ToList();
                        if (result?.Count > 0) productBindingSource.DataSource = new BindingList<Product>(result);
                    }
                    if (searchList.Count == 4)
                    {
                        var result = _products?.Where(
                            p => p.ProductName.ToLower().Contains(searchList[0].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[1].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[2].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[3].ToLower())).ToList();
                        if (result?.Count > 0) productBindingSource.DataSource = new BindingList<Product>(result);
                    }
                    if (searchList.Count == 5)
                    {
                        var result = _products?.Where(
                            p => p.ProductName.ToLower().Contains(searchList[0].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[1].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[2].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[3].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[4].ToLower())).ToList();
                        if (result?.Count > 0) productBindingSource.DataSource = new BindingList<Product>(result);
                    }
                    if (searchList.Count == 6)
                    {
                        var result = _products?.Where(
                            p => p.ProductName.ToLower().Contains(searchList[0].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[1].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[2].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[3].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[4].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[5].ToLower())).ToList();
                        if (result?.Count > 0) productBindingSource.DataSource = new BindingList<Product>(result);
                    }
                    if (searchList.Count == 7)
                    {
                        var result = _products?.Where(
                            p => p.ProductName.ToLower().Contains(searchList[0].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[1].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[2].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[3].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[4].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[5].ToLower()) ||
                            p.ProductName.ToLower().Contains(searchList[6].ToLower())).ToList();
                        if (result?.Count > 0) productBindingSource.DataSource = new BindingList<Product>(result);
                    }
                }

            }
        }
    }
}