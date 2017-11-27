using System;
using System.Web;
using System.Web.UI;
using InformTmLiveStats;

namespace InformIntranet
{
	public partial class Default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				var informad = new ActiveDirectoryHelper.ActiveDirectoryHelper();
				var username = HttpContext.Current.Request.LogonUserIdentity.Name.Substring(HttpContext.Current.Request.LogonUserIdentity.Name.IndexOf(@"\") + 1);
				var userobj = informad.GetUserByLoginName(username);

				// check for user group
				var isTeleMarketer = informad.IsInGroup(username, "Telemarketers") || informad.IsInGroup(username, "Permanent Telemarketers");
				if (isTeleMarketer)
				{
					var statsTable = LiveStatsService.GetTMInfoStats(userobj.Name, "Stats Sydney");

					if (statsTable.Rows.Count > 0)
					{
						litCallsMade.Text = statsTable.Rows[0][0].ToString();
						double connectedMinutes = Convert.ToInt32(statsTable.Rows[0][1]);
						connectedMinutes = Math.Round(connectedMinutes);
						litConnectedMinutes.Text = connectedMinutes.ToString();
						DateTime firstcall = Convert.ToDateTime(statsTable.Rows[0][3]);
						litFirstCall.Text = firstcall.ToShortTimeString();
						litExtension.Text = statsTable.Rows[0][2].ToString();
					}
					else
					{
						statsTable = LiveStatsService.GetTMInfoStats(userobj.Name, "Stats Manila");

						if (statsTable.Rows.Count > 0)
						{
							litCallsMade.Text = statsTable.Rows[0][0].ToString();
							double connectedMinutes = Convert.ToInt32(statsTable.Rows[0][1]);
							connectedMinutes = Math.Round(connectedMinutes);
							litConnectedMinutes.Text = connectedMinutes.ToString();
							DateTime firstcall = Convert.ToDateTime(statsTable.Rows[0][3]);
							litFirstCall.Text = firstcall.ToShortTimeString();
							litExtension.Text = statsTable.Rows[0][2].ToString();
						}
						else
						{
							pnlCallStats.Visible = false;
							pnlNonTM.Visible = true;
						}
					}
				}
				else
				{
					pnlCallStats.Visible = false;
					pnlNonTM.Visible = true;
				}
			}
		}
	}
}