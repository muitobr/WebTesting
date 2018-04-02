using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace WebScrappingTest
{
    public class PaginaClassificacao
    {
        private SeleniumConfigurations _configurations;
        private IWebDriver _driver;

        public PaginaClassificacao(SeleniumConfigurations configurations)
        {
            _configurations = configurations;

            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");
            _driver = new FirefoxDriver(_configurations.CaminhoDriverFirefox, options);
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_configurations.Timeout);
           _driver.Navigate().GoToUrl(_configurations.UrlPaginaClassificacaoNBA);
        }

        public List<Commander> ObterClassificacao()
        {
            DateTime dataCarga = DateTime.Now;
            List<Commander> baralho = new List<Commander>();

            string nomeComandante = _driver.FindElement(By.CssSelector("h3.panel-title")).Text;

            var dadosConferencias = _driver.FindElements(By.ClassName("cards"));
            var captions = _driver.FindElements(By.ClassName("card"));

            for (int i = 0; i < captions.Count; i++)
            {
                var caption = captions[i];
                Commander cartasCommander = new Commander();
                cartasCommander.Comandante = nomeComandante;
                cartasCommander.DataCarga = dataCarga;
                cartasCommander.Nome = nomeComandante;
                baralho.Add(cartasCommander);

                int posicao = 0;
                var conf = dadosConferencias[i];
                var dadosCartas = conf.FindElements(By.CssSelector("div.nwname"));
                foreach (var dadosCarta in dadosCartas)
                {
                    var estatisticasEquipe = conf.FindElements(By.CssSelector("div.nwdesc.ellipsis"));
                    posicao++;
                    Cartas carta = new Cartas();
                    carta.Posicao = posicao;
                    carta.Nome = dadosCartas[posicao].Text;
                    try
                    {
                        Console.WriteLine($"Extraindo {carta.Nome} da posicao {posicao}");
                        carta.percentualDecksSinergia = estatisticasEquipe[posicao].Text;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw;
                    }
                    finally
                    {
                        cartasCommander.Cards.Add(carta);
                    }

                   }
            }

            return baralho;
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
