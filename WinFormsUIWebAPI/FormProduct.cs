using PowerTools.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsUIWebAPI
{
    public partial class FormProduct : Form
    {
        private Product _product;
        public FormProduct(Product product)
        {
            InitializeComponent();
            _product = product;
            productBindingSource.DataSource = _product;
            loadImage(_product.ImageUrl);
        }
        async void loadImage(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                pictureBoxProduct.Image = await GetImageFromUrl(url);
            }
        }
        public async Task<Bitmap> GetImageFromUrl(string url)
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(url);
            return new Bitmap(stream);
        }
    }
}
