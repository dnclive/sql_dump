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
		public tsql_dump()
		{

		}

		public tsql_dump(t args)
		{
			//входные параметры

			t_msslq_cli mssql_cli = new t_msslq_cli();

			mssql_cli.f_connect(args);

			//вывод в global
			this["mssql_cli"] = mssql_cli;
		}

        public void f_db_arr(t args)
        {
            t_msslq_cli mssql_cli = this["mssql_cli"].f_val<t_msslq_cli>();

            //получаем список доступных бд для сервера
            //и выполняем f_each если передана
            mssql_cli.f_select(new t().f_add(true, args).f_add(true, new t()
            {
                {"cmd",          "exec sp_databases"},
                {"tab_name",     "tabs"},
                {
					"f_done_", new t_f<t,t>(delegate(t args_1)
					{
						DataTable tab=args_1["tab"].f_val<DataTable>();
					
						int i=-1;
						foreach(DataRow dr in tab.Rows)
						{
							i++;

							f_f("f_each", args.f_add(true, new t()
							{
								{"item",	dr["database_name"].ToString()},
								{"index",	i}
							}));
						}

						return null;
					})
				},
				{
					"f_each", new t_f<t,t>(delegate(t args_1)
					{
						DataRow dr=args_1["each"]["item"].f_val<DataRow>();
						int index = args_1["each"]["index"].f_val<int>();

						f_f("f_each", args.f_add(true, new t()
						{
							{"item",	dr["database_name"].ToString()},
							{"index",	index}
						}));

						return null;
					})
				}
            }));
        }

		public void f_tab_arr(t args)
		{
			t_msslq_cli mssql_cli = this["mssql_cli"].f_val<t_msslq_cli>();
			string db_name = args["db_name"].f_str();

			//получаем список доступных бд для сервера
			//и выполняем f_each если передана
			mssql_cli.f_select(new t().f_add(true, args).f_add(true, new t()
            {
                {"cmd",			"select * from sys.Tables"},
                {"tab_name",	"tabs"},
				{"db_name",		db_name},
				{
					"each", new t()
					{
						{"sort", "name ASC"}
					}
				},
                {
					"f_each", new t_f<t,t>(delegate(t args_1)
					{
						DataRow dr=args_1["each"]["item"].f_val<DataRow>();
						int index = args_1["each"]["index"].f_val<int>();

						f_f("f_each", args.f_add(true, new t()
						{
							{"item",	dr["name"].ToString()},
							{"index",	index}
						}));

						return null;
					})
				}
            }));
		}

		public void f_dump(t args)
		{
			t_msslq_cli mssql_cli = this["mssql_cli"].f_val<t_msslq_cli>();
			string tab_name = args["tab_name"].f_str();

			string ins_sql_str="";

			//получаем список доступных бд для сервера
			//и выполняем f_each если передана
			mssql_cli.f_select(new t().f_add(true,args).f_add(true,new t()
            {
                {"cmd",			"Select * from "+tab_name},
                {	//когда будет получена таблица
					"f_done", new t_f<t,t>(delegate(t args_1)
					{
						DataTable tab = args_1["tab"].f_val<DataTable>();

						//формируем inset запрос для строк полученной таблицы
						mssql_cli.f_make_ins_query(new t()
						{
							{"tab", tab},
							{	//когда запрос будет сформирован
								//возвращаем его наверх
								"f_done", args["f_done"].f_f()
							}
						});

						return null;
					})
				}
            }));
		}

	}
}
