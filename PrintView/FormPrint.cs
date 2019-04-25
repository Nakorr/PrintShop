using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintView
{
    public partial class FormPrint : Form
    {
            public int Id { set { id = value; } }
            private int? id;
            private List<PrintIngredientViewModel> PrintIngredients;

            public FormPrint()
            {
                InitializeComponent();
            }

            private void FormProduct_Load(object sender, EventArgs e)
            {
                if (id.HasValue)
                {
                    try
                    {
                        PrintViewModel view = APICustomer.GetRequest<PrintViewModel>("api/Print/Get/" + id.Value);
                        if (view != null)
                        {
                            textBoxName.Text = view.PrintName;
                            textBoxPrice.Text = view.Price.ToString();
                            PrintIngredients = view.PrintIngredients;
                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
                }
                else
                {
                    PrintIngredients = new List<PrintIngredientViewModel>();
                }
            }

            private void LoadData()
            {
                try
                {
                    if (PrintIngredients != null)
                    {
                        dataGridView.DataSource = null;
                        dataGridView.DataSource = PrintIngredients;
                        dataGridView.Columns[0].Visible = false;
                        dataGridView.Columns[1].Visible = false;
                        dataGridView.Columns[3].Visible = false;
                        dataGridView.Columns[2].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
            private void buttonAdd_Click(object sender, EventArgs e)
            {
                var form = new FormPrintIngredient();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.Model != null)
                    {
                        if (id.HasValue)
                        {
                            form.Model.PrintId = id.Value;
                        }
                        PrintIngredients.Add(form.Model);
                    }
                    LoadData();
                }
            }
            private void buttonUpd_Click(object sender, EventArgs e)
            {
                if (dataGridView.SelectedRows.Count == 1)
                {
                    var form = new FormPrintIngredient();
                    form.Model = PrintIngredients[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        PrintIngredients[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                        LoadData();
                    }
                }
            }
            private void buttonDel_Click(object sender, EventArgs e)
            {
                if (dataGridView.SelectedRows.Count == 1)
                {
                    if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            PrintIngredients.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                        }
                        LoadData();
                    }
                }
            }
            private void buttonRef_Click(object sender, EventArgs e)
            {
                LoadData();
            }

            private void buttonSave_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(textBoxName.Text))
                {
                    MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(textBoxPrice.Text))
                {
                    MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                    return;
                }
                if (PrintIngredients == null || PrintIngredients.Count == 0)
                {
                    MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    List<PrintIngredientBindingModel> PrintIngredientBM = new
                   List<PrintIngredientBindingModel>();
                    for (int i = 0; i < PrintIngredients.Count; ++i)
                    {
                        PrintIngredientBM.Add(new PrintIngredientBindingModel
                        {
                            Id = PrintIngredients[i].Id,
                            PrintId = PrintIngredients[i].PrintId,
                            IngredientId = PrintIngredients[i].IngredientId,
                            Count = PrintIngredients[i].Count
                        });
                    }
                    if (id.HasValue)
                    {
                        APICustomer.PostRequest<PrintBindingModel,
                        bool>("api/Print/UpdElement", new PrintBindingModel
                        {
                            Id = id.Value,
                            PrintName = textBoxName.Text,
                            Price = Convert.ToInt32(textBoxPrice.Text),
                            PrintIngredient = PrintIngredientBM
                        });
                    }
                    else
                    {
                        APICustomer.PostRequest<PrintBindingModel,
                        bool>("api/Print/AddElement", new PrintBindingModel
                        {
                            PrintName = textBoxName.Text,
                            Price = Convert.ToInt32(textBoxPrice.Text),
                            PrintIngredient = PrintIngredientBM
                        });
                    }
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }

            private void buttonCancel_Click(object sender, EventArgs e)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }