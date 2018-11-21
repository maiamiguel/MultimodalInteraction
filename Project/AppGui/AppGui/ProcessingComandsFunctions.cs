using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGui{
    class ProcessingComandsFunctions{

        private static Boolean firstTime = true;
        private static IWebDriver driver;
        private static IWebElement element;
        private static IWebElement element1;
        private static IWebElement source;
        private static String option = "";
        private static String[] M1Destinations =
        {
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=index&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Findex.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bsb_price_type%3Dtotal%26%3B&ss=Maldivas&is_ski_area=0&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bac_click_type%3Db%3Bac_position%3D0%3Bclass_interval%3D1%3Bdest_id%3D3978%3Bdest_type%3Dregion%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dregion%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dsearchresults%3Bsrc_elem%3Dsb%3Bsrpvid%3D70ec05b8dcbc0104%3Bss%3DBora%2520Bora%252C%2520Polin%25C3%25A9sia%2520Francesa%3Bss_all%3D0%3Bss_raw%3DBora%2520bora%3Bssb%3Dempty%3Bsshis%3D0%3Bssne_untouched%3DMaldivas%26%3B&ss=Bora+Bora&is_ski_area=0&ssne=Bora+Bora&ssne_untouched=Bora+Bora&region=3978&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bclass_interval%3D1%3Bdest_id%3D3978%3Bdest_type%3Dregion%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Bregion%3D3978%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dsearchresults%3Bsrc_elem%3Dsb%3Bsrpvid%3D70ec05be71cf027d%3Bss%3DBora%2520Bora%3Bss_all%3D0%3Bssb%3Dempty%3Bsshis%3D0%3Bssne%3DBora%2520Bora%3Bssne_untouched%3DBora%2520Bora%26%3B&ss=Tail%C3%A2ndia&is_ski_area=&ssne=Bora+Bora&ssne_untouched=Bora+Bora&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Tail%C3%A2ndia&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=209&dest_type=country&place_id_lat=14.3594&place_id_lon=100.883&search_pageview_id=70ec05be71cf027d&search_selected=true&search_pageview_id=70ec05be71cf027d&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0"
        };

        private static String[] M2Destinations =
        {
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=index&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Findex.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bsb_price_type%3Dtotal%26%3B&ss=Cabo+Verde&is_ski_area=&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=CAbo+verde&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=39&dest_type=country&place_id_lat=16.0251&place_id_lon=-24.1736&search_pageview_id=065d06cbf1a201d6&search_selected=true&search_pageview_id=065d06cbf1a201d6&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bac_click_type%3Db%3Bac_position%3D0%3Bclass_interval%3D1%3Bdest_id%3D39%3Bdest_type%3Dcountry%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dcountry%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dindex%3Bsrc_elem%3Dsb%3Bsrpvid%3D3f2906d01b460172%3Bss%3DCabo%2520Verde%3Bss_all%3D0%3Bss_raw%3DCAbo%2520verde%3Bssb%3Dempty%3Bsshis%3D0%26%3B&ss=M%C3%A9xico&is_ski_area=&ssne=Cabo+Verde&ssne_untouched=Cabo+Verde&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=M%C3%A9xico&ac_position=1&ac_langcode=pt&ac_click_type=b&dest_id=137&dest_type=country&place_id_lat=23.3446&place_id_lon=-102.188&search_pageview_id=3f2906d01b460172&search_selected=true&search_pageview_id=3f2906d01b460172&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bac_click_type%3Db%3Bac_position%3D1%3Bclass_interval%3D1%3Bdest_id%3D137%3Bdest_type%3Dcountry%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dcountry%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dsearchresults%3Bsrc_elem%3Dsb%3Bsrpvid%3Db9c106d8e4fd007d%3Bss%3DM%25C3%25A9xico%3Bss_all%3D0%3Bss_raw%3DM%25C3%25A9xico%3Bssb%3Dempty%3Bsshis%3D0%3Bssne_untouched%3DCabo%2520Verde%26%3B&ss=Brasil&is_ski_area=&ssne=M%C3%A9xico&ssne_untouched=M%C3%A9xico&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Brasil&ac_position=1&ac_langcode=pt&ac_click_type=b&dest_id=30&dest_type=country&place_id_lat=-11.0252&place_id_lon=-52.9666&search_pageview_id=b9c106d8e4fd007d&search_selected=true&search_pageview_id=b9c106d8e4fd007d&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0"
        };

        private static String[] M3Destinations =
        {
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=index&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Findex.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bsb_price_type%3Dtotal%26%3B&ss=Isl%C3%A2ndia&is_ski_area=&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Isl%C3%A2ndia&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=97&dest_type=country&place_id_lat=64.72&place_id_lon=-18.4377&search_pageview_id=bb7b06ffc00902e1&search_selected=true&search_pageview_id=bb7b06ffc00902e1&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bac_click_type%3Db%3Bac_position%3D0%3Bclass_interval%3D1%3Bdest_id%3D97%3Bdest_type%3Dcountry%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dcountry%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dindex%3Bsrc_elem%3Dsb%3Bsrpvid%3De27707030b1a002b%3Bss%3DIsl%25C3%25A2ndia%3Bss_all%3D0%3Bss_raw%3DIsl%25C3%25A2ndia%3Bssb%3Dempty%3Bsshis%3D0%26%3B&ss=Noruega&is_ski_area=&ssne=Isl%C3%A2ndia&ssne_untouched=Isl%C3%A2ndia&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Noruega&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=159&dest_type=country&place_id_lat=64.254&place_id_lon=11.6332&search_pageview_id=e27707030b1a002b&search_selected=true&search_pageview_id=e27707030b1a002b&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0" ,
            "https://www.booking.com/searchresults.pt-pt.html?label=gen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID&sid=271d1d31a5760bfd2f7feb85e9f9d675&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.pt-pt.html%3Flabel%3Dgen173nr-1DCAEoggI46AdIM1gEaLsBiAEBmAEfuAEXyAEM2AED6AEBiAIBqAID%3Bsid%3D271d1d31a5760bfd2f7feb85e9f9d675%3Bac_click_type%3Db%3Bac_position%3D0%3Bclass_interval%3D1%3Bdest_id%3D159%3Bdest_type%3Dcountry%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dcountry%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dsearchresults%3Bsrc_elem%3Dsb%3Bsrpvid%3Df06a071915530001%3Bss%3DNoruega%3Bss_all%3D0%3Bss_raw%3DNoruega%3Bssb%3Dempty%3Bsshis%3D0%3Bssne_untouched%3DIsl%25C3%25A2ndia%26%3B&ss=Su%C3%A9cia&is_ski_area=&ssne=Noruega&ssne_untouched=Noruega&checkin_monthday=&checkin_month=&checkin_year=&checkout_monthday=&checkout_month=&checkout_year=&no_rooms=1&group_adults=2&group_children=0&b_h4u_keep_filters=&from_sf=1&ss_raw=Su%C3%A9cia&ac_position=0&ac_langcode=pt&ac_click_type=b&dest_id=203&dest_type=country&place_id_lat=62.7545&place_id_lon=15.3198&search_pageview_id=f06a071915530001&search_selected=true&search_pageview_id=f06a071915530001&ac_suggestion_list_length=5&ac_suggestion_theme_list_length=0"
        };

        public static void AcceptCommand(String type, String country){
            
            if (firstTime){
                driver = new ChromeDriver();
                firstTime = false;                
            }

            // Selenium

            String countryName = null;

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

            if (type == "VOO")
            {
                option = "VOO";
                /*driver.Navigate().GoToUrl("https://booking.kayak.com/");
                IWebElement source = driver.FindElement(By.Name("origin"));
                source.SendKeys("lisboa");
                IWebElement element = driver.FindElement(By.Name("destination"));
                IWebElement element1 = driver.FindElement(By.Id("WZmS-submit"));
                */
                /*
                driver.Navigate().GoToUrl("https://www.momondo.pt/");
                source = driver.FindElement(By.Name("origin"));

                element = driver.FindElement(By.Name("destination"));
                element1 = driver.FindElement(By.Id("Zyw6-submit"));
                */

                /*
                driver.Navigate().GoToUrl("https://www.kiwi.com/pt/");
                source = driver.FindElement(By.Name("origin"));

                element = driver.FindElement(By.Name("destination"));
                element1 = driver.FindElement(By.Id("Zyw6-submit"));
                */

            }
            else if (type == "HOTEL")
            {
                driver.Navigate().GoToUrl("https://booking.com");           // Navigate to webpage
                element = driver.FindElement(By.Name("ss"));                // Find the Search text box UI Element
                element1 = driver.FindElement(By.ClassName("sb-searchbox__button"));    // Click the search button
                DateTime thisDay = DateTime.Today;
                var year = thisDay.Year;
                var month = thisDay.Month;
                var day = thisDay.Day;

                DateTime checkout = DateTime.Today;
                checkout.AddDays(4);
                var year2 = checkout.Year;
                var month2 = checkout.Month;
                var day2 = checkout.Day;

                driver.FindElement(By.Name("checkin_month")).SendKeys(month.ToString());
                driver.FindElement(By.Name("checkin_monthday")).SendKeys(day.ToString());
                driver.FindElement(By.Name("checkin_year")).SendKeys(year.ToString());


                driver.FindElement(By.Name("checkout_month")).SendKeys(month2.ToString());
                driver.FindElement(By.Name("checkout_monthday")).SendKeys(day2.ToString());
                driver.FindElement(By.Name("checkout_year")).SendKeys(year2.ToString());
            }
            else if (type == "FOOD")
            {
                driver.Navigate().GoToUrl("https://www.opentable.co.uk/?ref=13850");        // Navigate to booking page
                element = driver.FindElement(By.ClassName("dtp-picker-search tt-input"));   // Find the Search text box UI Element
                element1 = driver.FindElement(By.ClassName("button dtp-picker-button"));    // Click the search button
            }
            else if (type == "CARS")
            {
                driver.Navigate().GoToUrl("http://www.rentalcars.com/");                // Navigate to booking page
                element = driver.FindElement(By.ClassName("ui-autocomplete-input"));    //Find the Search text box UI Element
                element1 = driver.FindElement(By.Id("formsubmit"));                     // Click the search button
            }
            else if (type == "CLOSE")
            {
                Console.WriteLine("FECHAR O BROWSER!");
                driver.Close();
                firstTime = true;
            }

            switch (country){
                case "SPAIN":
                    countryName = "Espanha";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "PORTUGAL":
                    countryName = "Portugal";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "ITALY":
                    countryName = "Itália";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "SWITZERLAND":
                    countryName = "Suíça";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "HOLAND":
                    countryName = "Holanda";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "GERMAN":
                    countryName = "Alemanha";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "FRANCE":
                    countryName = "França";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "AUSTRIA":
                    countryName = "Aústria";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "CROATIA":
                    countryName = "Croácia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "SERVIA":
                    countryName = "Sérvia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "GREECE":
                    countryName = "Grécia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "BELGIUM":
                    countryName = "Bélgica";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "POLAND":
                    countryName = "Polónia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "HUNGARY":
                    countryName = "Hungria";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;
            }
        }
    }
}
