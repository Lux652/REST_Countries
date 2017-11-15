using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WindowsFormCountries
{
    public partial class Form1 : Form
    {
        public List<Country> lCountries;
        public Form1()
        {
            InitializeComponent();
            /*DATAGRID*/
            lCountries = GetCountries();
            dataGridViewCountries.DataSource = lCountries;

            /*COMBO BOX*/
            List<String> lRegions = lCountries.Where(o => o.sRegion != "").Select(o => o.sRegion).Distinct().ToList();
            lRegions.Insert(0, "Svi kontinenti");
            comboBoxRegion.DataSource = lRegions;

            List<string> lSortCriterias = new List<string>()
            {
                "‐",
                "Glavni grad",
                "Naziv",
                "Broj stanovnika",
                "Povrsina"
            };
            comboBoxSort.DataSource = lSortCriterias;

        }
        public List<Country> GetCountries()
        {
            List<Country> lRESTCountries = new List<Country>();
            string sUrl = System.Configuration.ConfigurationManager.AppSettings["RestApiUrl"];
            string sJson = CallRestMethod(sUrl);
            JArray json = JArray.Parse(sJson);
            foreach (JObject item in json)
            {
                string code = (string)item.GetValue("alpha2Code");
                string name = (string)item.GetValue("name");
                string capital = (string)item.GetValue("capital");
                int population = (int)item.GetValue("population");
                //fArea = (float)oCountries[i]["area"]
                float area = -1;
                if (item.GetValue("area").Type == JTokenType.Null)
                {
                    area = 0;
                }
                else
                {
                    area = (float)item.GetValue("area");
                }
                string region = (string)item.GetValue("region");

                lRESTCountries.Add(new Country
                {
                    sCode = code,
                    sName = name,
                    sCapital = capital,
                    nPopulation = population,
                    fArea = area,
                    sRegion = region,
                });
            }

            return lRESTCountries;
        }





        public static string CallRestMethod(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method ="GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            //webrequest.Headers.Add("Username", "xyz");
            //webrequest.Headers.Add("Password", "abc");
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(),enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }

        private void comboBoxRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sRegion = (string)comboBoxRegion.SelectedItem; // odabrana vrijednost
            lCountries = GetCountries();
            if (sRegion != "Svi kontinenti")
            {
                lCountries = lCountries.Where(o => o.sRegion == sRegion).ToList();

                dataGridViewCountries.DataSource = lCountries;
            }
            else
            {
                dataGridViewCountries.DataSource = lCountries;
            }
        }

        private void comboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sChoice = (string)comboBoxSort.SelectedItem;
            lCountries = GetCountries();

            switch(sChoice)
            {
                case "Glavni grad":
                    dataGridViewCountries.DataSource = lCountries.OrderBy(o => o.sCapital).ToList();
                    break;
                case "Naziv":
                    dataGridViewCountries.DataSource = lCountries.OrderBy(o => o.sCapital).ToList();
                    break;
                case "Broj stanovnika":
                    dataGridViewCountries.DataSource = lCountries.OrderByDescending(o => o.nPopulation).ToList();
                    break;
                case "Povrsina":
                    dataGridViewCountries.DataSource = lCountries.OrderByDescending(o => o.fArea).ToList();
                    break;
                default:
                    dataGridViewCountries.DataSource = lCountries;
                    break;
            }


        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string sSearch = (string)textBoxSearch.Text;
            dataGridViewCountries.DataSource = lCountries.Where(o => o.sName.Contains(sSearch)).ToList();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string sKod = (string)textBoxCode.Text;
            string sNaziv = (string)textBoxNaziv.Text;
            string sGlavniGrad = (string)textBoxGrad.Text;
            int nBrojStanovnika = Convert.ToInt32(textBoxStan.Text);
            float fPovrsina = Convert.ToSingle(textBoxPovrsina.Text);
            string sKontinent = (string)textBoxKont.Text;
            lCountries.Add(new Country
            {
                sCode = sKod,
                sName = sNaziv,
                sCapital = sGlavniGrad,
                nPopulation = nBrojStanovnika,
                fArea = fPovrsina,
                sRegion = sKontinent
            });
            dataGridViewCountries.DataSource = lCountries;
        }
    }
}
