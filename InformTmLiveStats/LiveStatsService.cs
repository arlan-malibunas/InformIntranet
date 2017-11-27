using System;
using System.Data;
using System.Data.Odbc;

namespace InformTmLiveStats
{
	public class LiveStatsService
	{
		public static DataTable GetTMInfoStats(string username, string DSN)
		{
			OdbcConnection cn;
			OdbcDataAdapter da;
			var table = new DataTable();
			string today = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
			var tomorrowDT = DateTime.Now.AddDays(1);
			var tomorrow = Convert.ToString(tomorrowDT.Year + "-" + tomorrowDT.Month + "-" + tomorrowDT.Day);
			table.TableName = "InfoStats";

			var qry = "SELECT count(*) as Calls, sum(billsec)/60 as Minutes, SUBstring(channel,7,3) as Extension, min(calldate) as FirstCall FROM cdr WHERE SUBSTR(clid, 2, LOCATE('\"', clid, 2) - 2) = '" +
					  username + "' AND calldate >= '" + today + "' AND calldate < '" + tomorrow + "' GROUP BY Extension ORDER BY Minutes DESC";
			cn = new OdbcConnection("dsn=" + DSN);
			cn.Open();
			da = new OdbcDataAdapter(qry, cn);
			table = new DataTable();
			table.TableName = "Stats";
			da.Fill(table);
			cn.Close();
			return table;
		}
	}
}