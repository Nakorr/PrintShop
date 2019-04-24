using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;


namespace PrintView
{
    public partial class FormStocksLoad : Form
    {

        public FormStocksLoad()
        {
            InitializeComponent();
        }
        private void FormStocksLoad_Load(object sender, EventArgs e)
        {
            try
            {
                List<StocksLoadViewModel> dict = APICustomer.GetRequest<List<StocksLoadViewModel>>("api/Main/GetStocksLoad");
                if (dict != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var elem in dict)
                    {
                        dataGridView.Rows.Add(new object[] { elem.StockName, "", "" });
                        foreach (var listElem in elem.Ingredients)
                        {
                            dataGridView.Rows.Add(new object[] { "", listElem.Item1, listElem.Item2 });
                        }
                        dataGridView.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }
        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    APICustomer.PostRequest<ReportBindingModel, bool>("api/Main/SaveStocksLoad", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }
    }
}
