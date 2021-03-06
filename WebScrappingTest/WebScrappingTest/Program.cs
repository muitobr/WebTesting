﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WebScrappingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Carregando configurações...");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");
            var configuration = builder.Build();

            var seleniumConfigurations = new SeleniumConfigurations();
            new ConfigureFromConfigurationOptions<SeleniumConfigurations>(
                configuration.GetSection("SeleniumConfigurations")).Configure(seleniumConfigurations);

            Console.WriteLine("Carregando driver do Selenium para Firefox em modo headless...");
            var obterListaDeCartas = new ObterPagina(seleniumConfigurations);

            Console.WriteLine("Carregando página EDHREC...");
            obterListaDeCartas.CarregarPagina();

            Console.WriteLine("Extraindo dados...");
            var classificacao = obterListaDeCartas.ObterDeckList();
            obterListaDeCartas.Fechar();

            Console.WriteLine("Gravando dados extraídos...");
            new CommanderRepository(configuration).Incluir(classificacao);
            Console.WriteLine("Carga de dados concluída com sucesso!");

            Console.ReadKey();
        }
    }
}
