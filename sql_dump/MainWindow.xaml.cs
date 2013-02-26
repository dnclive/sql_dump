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

        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
			f_cre_sql_dump();
        }

		private tsql_dump f_cre_sql_dump()
		{
			if (args["sql_dump"].f_val() != null)
			{
				return args["sql_dump"].f_val<tsql_dump>();
			}
			tsql_dump sql_dump = new tsql_dump(new t()
			{
                {"server",       txt_server.Text},
                {"server_name",  txt_server_name.Text},
                {"login",        txt_login.Text},
                {"pass",         txt_pass.Text},
				{
					"f_done", new t_f<t,t>(delegate(t args_1)
					{

						txt_out.Text=args_1["message"].f_str();
						

						return null;
					})
				},
				{
					"f_fail", new t_f<t,t>(delegate(t args_1)
					{

						SqlException ex = args_1["ex"].f_val<SqlException>();

						txt_out.Text=ex.Message+"\r\n";
						txt_out.Text=args_1["message"].f_str()+"\r\n";

						return null;
					})
				}
			});

			//выводим в глобал
			args["sql_dump"] = new t(sql_dump);

			return sql_dump;
		}

        private void chb_db_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            tsql_dump sql_dump = f_cre_sql_dump();

			//если в списке уже есть элементы - уже запрашивали
			if (chb_db.Items.Count > 0)
			{
				return;
			}

            //получаем список доступных БД для сервера
            //и запрашиваем 
            sql_dump.f_db_arr(new t()
            {
				{"server",       txt_server.Text},
                {"server_name",  txt_server_name.Text},
                {"login",        txt_login.Text},
                {"pass",         txt_pass.Text},
                {
					"f_each", new t_f<t,t>(delegate(t args_1)
					{

						string item = args_1["item"].f_val<string>();
						int i = args_1["index"].f_val<int>();

						chb_db.Items.Add(item);

						return null;
					})
				},
				{"f_fail", new t_f<t,t>(f_fail)}
            });

        }

		private void chb_table_name_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ComboBox chb = (ComboBox)sender;

			tsql_dump sql_dump = f_cre_sql_dump();

			//если в списке уже есть элементы - уже запрашивали
			if (chb.Items.Count > 0)
			{
				return;
			}

			//получаем список доступных БД для сервера
			//и запрашиваем 
			sql_dump.f_tab_arr(new t()
            {
				{"db_name", chb_db.SelectedItem.ToString()},
                {
					"f_each", new t_f<t,t>(delegate(t args_1)
					{

						string item = args_1["item"].f_val<string>();
						int i = args_1["index"].f_val<int>();

						chb.Items.Add(item);

						return null;
					})
				},
				{"f_fail", new t_f<t,t>(f_fail)},
				{
					"f_fail_", new t_f<t,t>(delegate(t args_1)
					{

						SqlException ex = args_1["ex"].f_val<SqlException>();

						txt_out.Text=ex.Message+"\r\n";
						txt_out.Text=args_1["message"].f_str()+"\r\n";

						return null;
					})
				}
				
            });
		}

		private void btn_dump_Click(object sender, RoutedEventArgs e)
		{
			tsql_dump sql_dump = f_cre_sql_dump();

			sql_dump.f_dump(new t()
			{
				{"tab_name", chb_table_name.SelectedItem.ToString()},
				{
					"f_done", new t_f<t,t>(delegate(t args_1)
					{
						string query = args_1["query"].f_val<string>();

						txt_out.Text = query;

						return null;
					})
				},
				{"f_fail", new t_f<t,t>(f_fail)}
			});
		}

		private t f_fail(t args)
		{
			SqlException ex = args["ex"].f_def_set(new Exception("ex не задан")).f_val<SqlException>();

			txt_out.Text=ex.Message+"\r\n";
			txt_out.Text=args["message"].f_str()+"\r\n";

			return null;
		}

    }
}
