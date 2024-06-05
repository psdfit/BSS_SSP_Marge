using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Services
{
 
        public class CalculateDateDifference

        {
            public string DatetimeCount(DateTime CreatedDate) {


            DateTime dt1 = DateTime.Now;

              double totalDays = (dt1 - CreatedDate).TotalDays;
                totalDays= Math.Round(totalDays);

            string diff = string.Empty;

                if (totalDays<=30)
                {

                    if (totalDays == 0)
                    { diff = "Today"; }
                   else if (totalDays == 1)
                     { diff += +totalDays + " " + "day ago"; }
                else
                    { diff += +totalDays +" "+ "days ago"; }

                }
            else
            {
                diff = CreatedDate.ToString("dddd, dd MMMM yyyy");
            }
                
                return  diff;
            
        }
        }
}
