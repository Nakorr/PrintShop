using PrintShopServiceDAL.Interfaces;
using PrintShopServiceImplement.Implementations;
using PrintShopServiceImplementDataBase;
using PrintShopServiceImplementDataBase.Implementations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PrintView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            APICustomer.Connect();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

    }
}