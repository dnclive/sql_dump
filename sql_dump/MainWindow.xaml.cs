using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Drawing;
using tlib;

namespace sql_dump
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        t args=new t();

        public MainWindow()
        {
            InitializeComponent();

			args["sql_dump"] = new t(new tsql_dump());



        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
			tsql_dump sql_dump = args["sql_dump"].f_val<tsql_dump>();

			sql_dump.f_connect(new t()
			{

			});

        }

		private void txt_server_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
    }
}
