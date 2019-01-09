using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace AppGui
{
    class ProcessingCommandFunctions
    {
        private static Boolean firstTime = true;
        private static IWebDriver driver;
        private static Actions action;
        private static Boolean firstFlightSearch = true;
        private static Boolean firstHotelSearch = true;

        private static String[] M1Destinations =
        {
            "https://www.cleartrip.com/hotels/results?city=Bora+Bora&state=&country=PF&area=&poi=&hotelId=&hotelName=&SearchTag=&dest_code=400386&chk_in=17/01/2019&chk_out=22/01/2019&adults1=2&children1=0&num_rooms=1" ,
            "https://www.cleartrip.com/hotels/results?city=Maldives&state=&country=MV&area=&poi=&hotelId=&hotelName=&SearchTag=&dest_code=446178&chk_in=17/01/2019&chk_out=22/01/2019&adults1=2&children1=0&num_rooms=1",
            "https://www.cleartrip.com/hotels/results?city=Honolulu&state=HI&country=US&area=&poi=&hotelId=&hotelName=&SearchTag=&dest_code=438520&chk_in=17/01/2019&chk_out=22/01/2019&adults1=2&children1=0&num_rooms=1"
        };

        private static String[] M2Destinations =
        {
            "https://www.cleartrip.com/flights/international/results?from=OPO&to=MIA&depart_date=21/12/2018&adults=1&childs=0&infants=0&class=Economy&airline=&carrier=&intl=y&sd=1544744588946",
            "https://www.cleartrip.com/flights/international/results?from=OPO&to=LAX&depart_date=25/12/2018&adults=1&childs=0&infants=0&class=Economy&airline=&carrier=&intl=y&sd=1544744630594",
            "https://www.cleartrip.com/flights/international/results?from=OPO&to=IBZ&depart_date=28/12/2018&adults=1&childs=0&infants=0&class=Economy&airline=&carrier=&intl=y&sd=1544744668487&page=loaded"
        };

        private static String[] M3Destinations =
        {
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=index&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Findex.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bsb_price_type%3Dtotal%26%3B&ss=Isl%C3%A2ndia&is_ski_area=&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Isl%C3%A2ndia&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=97&dest_type=country&place_id_lat=64.72&place_id_lon=-18.4377&search_pageview_id=bb7b06ffc00902e1&search_selected=true&search_pageview_id=bb7b06ffc00902e1&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bac_click_type%3Db%3Bac_position%3D0%3Bclass_interval%3D1%3Bdest_id%3D97%3Bdest_type%3Dcountry%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dcountry%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dindex%3Bsrc_elem%3Dsb%3Bsrpvid%3De27707030b1a002b%3Bss%3DIsl%25C3%25A2ndia%3Bss_all%3D0%3Bss_raw%3DIsl%25C3%25A2ndia%3Bssb%3Dempty%3Bsshis%3D0%26%3B&ss=Noruega&is_ski_area=&ssne=Isl%C3%A2ndia&ssne_untouched=Isl%C3%A2ndia&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Noruega&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=159&dest_type=country&place_id_lat=64.254&place_id_lon=11.6332&search_pageview_id=e27707030b1a002b&search_selected=true&search_pageview_id=e27707030b1a002b&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bac_click_type%3Db%3Bac_position%3D0%3Bclass_interval%3D1%3Bdest_id%3D159%3Bdest_type%3Dcountry%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dcountry%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dsearchresults%3Bsrc_elem%3Dsb%3Bsrpvid%3Df06a071915530001%3Bss%3DNoruega%3Bss_all%3D0%3Bss_raw%3DNoruega%3Bssb%3Dempty%3Bsshis%3D0%3Bssne_untouched%3DIsl%25C3%25A2ndia%26%3B&ss=Su%C3%A9cia&is_ski_area=&ssne=Noruega&ssne_untouched=Noruega&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Su%C3%A9cia&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=203&dest_type=country&place_id_lat=62.7545&place_id_lon=15.3198&search_pageview_id=f06a071915530001&search_selected=true&search_pageview_id=f06a071915530001&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0"
        };
        private static string travellers;

        public static void AcceptCommand(String type, String city){
            if (firstTime)
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                //driver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromSeconds(30));
                firstTime = false;
            }

            if (type == "SEARCH")
            {
                if (city == "FLIGHT")
                {
                    // go to home page
                    driver.Navigate().GoToUrl("https://www.cleartrip.com/");
                    // click country button
                    driver.FindElement(By.XPath("//*[@id='userAccountNav']/nav/ul/li[2]/a")).Click();
                    // select US country
                    driver.FindElement(By.XPath("//*[@id='countryForm']/li[8]/a")).Click();
                }
                else if (city == "HOTEL")
                {
                    // go to home page
                    driver.Navigate().GoToUrl("https://www.cleartrip.com/");
                    // click country button
                    driver.FindElement(By.XPath("//*[@id='userAccountNav']/nav/ul/li[2]/a")).Click();
                    // select US country
                    driver.FindElement(By.XPath("//*[@id='countryForm']/li[8]/a")).Click();
                    // click hotels link
                    driver.FindElement(By.XPath("//*[@id='Home']/div/aside[1]/nav/ul[1]/li[2]/a")).Click();
                }
            }

            if (type == "M1")
            {
                Random rnd = new Random();
                int rand = rnd.Next(0, 2);
                driver.Navigate().GoToUrl(M1Destinations[rand]);
            }

            if (type == "M2")
            {
                Random rnd = new Random();
                int rand = rnd.Next(0, 2);
                driver.Navigate().GoToUrl(M2Destinations[rand]);
            }

            if (type == "M3")
            {
                Random rnd = new Random();
                int rand = rnd.Next(0, 2);
                driver.Navigate().GoToUrl(M3Destinations[rand]);
            }

            if (type == "CLOSE")
            {
                Console.WriteLine("FECHAR O BROWSER!");
                driver.Close();
                Environment.Exit(0);

                //firstTime = true;
            }

            if (type == "FLIGHT")
            {
                // go to home page
                driver.Navigate().GoToUrl("https://www.cleartrip.com/");
                // click country button
                driver.FindElement(By.XPath("//*[@id='userAccountNav']/nav/ul/li[2]/a")).Click();
                // select US country
                driver.FindElement(By.XPath("//*[@id='countryForm']/li[8]/a")).Click();

                switch (city)
                {
                    case "ROME":
                        // send source (default)
                        driver.FindElement(By.Id("FromTag")).Clear();
                        driver.FindElement(By.Id("FromTag")).SendKeys("Lisbon, PT - Lisboa (LIS)");
                        // send destination
                        driver.FindElement(By.Id("ToTag")).Clear();
                        driver.FindElement(By.Id("ToTag")).SendKeys("Rome, IT - Fiumicino (FCO)");
                        break;
                    case "PARIS":
                        // send source (default)
                        driver.FindElement(By.Id("FromTag")).Clear();
                        driver.FindElement(By.Id("FromTag")).SendKeys("Lisbon, PT - Lisboa (LIS)");
                        // send destination
                        driver.FindElement(By.Id("ToTag")).Clear();
                        driver.FindElement(By.Id("ToTag")).SendKeys("Paris, FR - Charles De Gaulle (CDG)");
                        break;

                    case "LONDON":
                        // send source (default)
                        driver.FindElement(By.Id("FromTag")).Clear();
                        driver.FindElement(By.Id("FromTag")).SendKeys("Lisbon, PT - Lisboa (LIS)");
                        // send destination
                        driver.FindElement(By.Id("ToTag")).Clear();
                        driver.FindElement(By.Id("ToTag")).SendKeys("London, GB - All airports (LON)");
                        break;
                }

                // click depart box
                driver.FindElement(By.XPath("//*[@id='DepartDate']")).Click();
                // click next month button
                if (firstFlightSearch)
                {
                    driver.FindElement(By.XPath("//*[@id='ui-datepicker-div']/div[2]/div/a")).Click();
                    firstFlightSearch = false;
                }
                // select depart day
                driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[1]/table/tbody/tr/td/a)[29]")).Click();
                // click search button
                driver.FindElement(By.Id("SearchBtn")).Click();
            }
            else if(type == "HOTEL")
            {
                // go to home page
                driver.Navigate().GoToUrl("https://www.cleartrip.com/");
                // click country button
                driver.FindElement(By.XPath("//*[@id='userAccountNav']/nav/ul/li[2]/a")).Click();
                // select US country
                driver.FindElement(By.XPath("//*[@id='countryForm']/li[8]/a")).Click();
                // click hotels link
                driver.FindElement(By.XPath("//*[@id='Home']/div/aside[1]/nav/ul[1]/li[2]/a")).Click();

                switch (city)
                {
                    case "ROME":
                        // send destination
                        driver.FindElement(By.Name("from")).Click();
                        driver.FindElement(By.Name("from")).SendKeys("Rome");
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // instantiate action object to press keys
                        action = new Actions(driver);
                        // we need to first press Down and Up keys, otherwise it won't work
                        action.SendKeys(Keys.Down).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        action.SendKeys(Keys.Up).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // press Enter key
                        action.SendKeys(Keys.Enter).Build().Perform();
                        break;

                    case "PARIS":
                        // send destination
                        driver.FindElement(By.Name("from")).Click();
                        driver.FindElement(By.Name("from")).SendKeys("Paris");
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // instantiate action object to press keys
                        action = new Actions(driver);
                        // we need to first press Down and Up keys, otherwise it won't work
                        action.SendKeys(Keys.Down).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        action.SendKeys(Keys.Up).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // press Enter key
                        action.SendKeys(Keys.Enter).Build().Perform();
                        break;

                    case "LONDON":
                        // send destination
                        driver.FindElement(By.Name("from")).Click();
                        driver.FindElement(By.Name("from")).SendKeys("London");
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // instantiate action object to press keys
                        action = new Actions(driver);
                        // we need to first press Down and Up keys, otherwise it won't work
                        action.SendKeys(Keys.Down).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        action.SendKeys(Keys.Up).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // press Enter key
                        action.SendKeys(Keys.Enter).Build().Perform();
                        break;
                }

                if (travellers == "O1")
                {
                    driver.FindElement(By.Id("travellersOnhome")).Click();
                    driver.FindElement(By.XPath("(//*[@id='travellersOnhome']/option[2]")).Click();
                }

                // click check-in box
                driver.FindElement(By.XPath("//*[@id='CheckInDate']")).Click();
                // click next month button
                if (firstHotelSearch)
                {
                    driver.FindElement(By.XPath("//*[@id='ui-datepicker-div']/div[2]/div/a")).Click();
                }
                // select check-in day
                driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[1]/table/tbody/tr/td/a)[29]")).Click();
                // click check-out box
                driver.FindElement(By.XPath("//*[@id='CheckOutDate']")).Click();
                // select check-out day
                if (firstHotelSearch)
                {
                    driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[2]/table/tbody/tr/td/a)[1]")).Click();
                    firstHotelSearch = false;
                }
                else
                {
                    driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[1]/table/tbody/tr/td/a)[1]")).Click();
                }
                // click search button
                driver.FindElement(By.Id("SearchHotelsButton")).Click();
            }


            if (type == "O1")
            {
                travellers = "O1";
            }

            if (type == "O2")
            {
                travellers = "O2";
            }

            if (type == "O3")
            {
                travellers = "O3";
            }
        }
    }
}
