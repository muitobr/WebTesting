using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace WebScrappingTest
{
    public class ObterPagina
    {
        private SeleniumConfigurations _configurations;
        private IWebDriver _driver;

        public ObterPagina(SeleniumConfigurations configurations)
        {
            _configurations = configurations;

            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _driver = new FirefoxDriver(_configurations.CaminhoDriverFirefox, options);
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_configurations.Timeout);
            _driver.Navigate().GoToUrl(_configurations.UrlPagina);
        }

        public List<Commander> ObterDeckList()
        {
            DateTime dataCarga = DateTime.Now;
            List<Commander> Deck = new List<Commander>();
            string nomeComandante = _driver.FindElement(By.CssSelector("h3.panel-title")).Text;
            var dadosConferencias = _driver.FindElements(By.ClassName("cards"));
            var captions = _driver.FindElements(By.ClassName("nwname"));

            Commander cartasCommander = new Commander();
            cartasCommander.Comandante = nomeComandante;
            cartasCommander.DataCarga = dataCarga;
            Deck.Add(cartasCommander);
            Console.WriteLine($"Extraindo: {nomeComandante} dia: {dataCarga}");

            int posicao = 0;
            var dadosCartas = _driver.FindElements(By.CssSelector("div.nwname"));
            foreach (var dadosCarta in dadosCartas)
            {
                var estatisticasCarta = _driver.FindElements(By.CssSelector("div.nwdesc.ellipsis"));
                Cartas carta = new Cartas();
                carta.Posicao = posicao;
                carta.Nome = dadosCartas[posicao].Text;
                Console.WriteLine($"Extraindo {carta.Nome} da posicao {posicao}");
                string percentualDecksSinergia = estatisticasCarta[posicao].Text;
                if (percentualDecksSinergia.Contains("\r\n"))
                {
                    string[] values = percentualDecksSinergia.Split("\r\n");
                    carta.percentualDecks = values[0].ToString();
                    carta.percentualSinergia = values[1].ToString();
                }
                else
                {
                    carta.percentualDecks = percentualDecksSinergia;
                    carta.percentualDecks = percentualDecksSinergia;
                }
                cartasCommander.Cards.Add(carta);
                posicao++;
            }
            return Deck;
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
