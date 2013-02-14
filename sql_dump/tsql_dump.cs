using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Drawing;
using tlib;
namespace sql_dump
{
	class tsql_dump:t
	{
		public void f_connect(t args)
		{
			//входные параметры
			string server = args["server"].f_str();
			string server_name = args["server_name"].f_str();
			string login = args["login"].f_str();
			string pass = args["pass"].f_str();
			
			//формируем строку подключения, без указания конкретной БД
			string sql_conn_str =	"Server=" + server + "/" + server_name + ";Database=;User Id=" + login + ";Password=" + pass;

			//создаем подключение
			SqlConnection sql_conn = new SqlConnection(sql_conn_str);
			
			

			//выносим в global нашего объекта
			this["sql_conn_str"] = new t(sql_conn_str);
			this["sql_conn"] = new t(sql_conn);


			//t.f_fdone(args["fdone"], new t());

		}

		public void f_exec_cmd(t args)
		{
			string			cmd_text =		args["cmd"].f_str();
			SqlConnection	conn =			this["sql_connection"].f_val<SqlConnection>();

			SqlCommand cmd = new SqlCommand(cmd_text, conn);


			

			//chb_db.Items.Add();
		}

		public void f_select(t args)
		{

			string cmd_text = args["cmd"].f_str();
			string tab_name = args["tab_name"].f_str();

			SqlConnection conn = this["sql_connection"].f_val<SqlConnection>();

			//t_f<t, t> f_done = args["f_done"].f_f<t_f>();

			//создаем адаптек для запроса
			SqlDataAdapter ad= new SqlDataAdapter(cmd_text, conn);

			//создаем таблицу для результата
			DataTable tab = new DataTable(tab_name);

			//получаем данные, заполняем таблицу
			ad.Fill(tab);

			//вкидываем в принятые параметры полеченную таблицу и возвращаем результат
			//в функцию обратного вызова

			args["tab"] = new t(tab);

			t_uti.f_fdone(args);

			return;
		}

	}
}
