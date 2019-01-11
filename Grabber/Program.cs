using Bot.Instagram.Profile;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Call();
        }

        private static void Call()
        {
            Console.WriteLine("[1] --> Twitter");
            Console.WriteLine("[2] --> Instagram");

            Console.WriteLine("\nInsira a opção desejada (1 ou 2)");

            var resposta = Console.ReadLine();

            try
            {
                switch (resposta)
                {
                    case "1":
                        Twitter();
                        break;
                    case "2":
                        IG();
                        break;

                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Insira um valor valido (1 ou 2)\n");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Call();
                        break;
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("O usuario não pode ser nulo, Tente novamente...");
                Console.ForegroundColor = ConsoleColor.Green;

                Call();

            }
            catch (WebException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Voce inseriu um usuario que nao existe ou nao é valido, tente novamente...");
                Console.ForegroundColor = ConsoleColor.Green;

                Call();

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ocorreu um erro desconhecido:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Tente novamente:");
                Console.ForegroundColor = ConsoleColor.Green;

                Call();

            }
        }

        private static void IG()
        {
            Console.WriteLine("Insira o user da pessoa");
            Console.Write(">");
            string userIg = Console.ReadLine();
            Console.Clear();
            if (userIg == "")
            {
                throw new ArgumentNullException("Insira um usuario valido");
            }
            Console.Clear();

            Profile profile = Instagram.GetProfileByUser(userIg);

            Console.WriteLine($"UserName = {profile.UserName}");
            Console.WriteLine($"\nTitle = {profile.Title}");
            Console.WriteLine($"\nProfile description = {profile.Description}");
            Console.WriteLine($"\nUrl = {profile.Url}");
            Console.WriteLine($"\nProfiile image = {profile.Image}");

            Select();

        }

        private static void Select()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n\n\nPressione (Enter) para Sair ou pressione (Esc) para voltar ao menu");
            Console.ForegroundColor = ConsoleColor.Green;
            var escolha = Console.ReadKey();


            if (escolha.Key == ConsoleKey.Escape)
            {
                Console.Clear();
                Call();
            }

            if (escolha.Key == ConsoleKey.Enter)
            {
                Console.WriteLine("Bye :)");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private static void Twitter()
        {
            Console.WriteLine("Insira o usario ex.@lolgamarco2");
            Console.Write(">");
            var userInformado = Console.ReadLine();
            Console.Clear();
            string url = @"https://twitter.com/" + userInformado.Replace("@", "");
            string markup;

            using (WebClient c = new WebClient())
            {
                markup = c.DownloadString(url);  //Webclient para baixar o html
            }

            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(markup);

            var profileDescription = html.DocumentNode.SelectNodes("//p[contains(@class, 'ProfileHeaderCard-bio u-dir')]");
            var name = html.DocumentNode.SelectNodes("//h1[contains(@class, 'ProfileHeaderCard-name')]//a");
            var user = html.DocumentNode.SelectNodes("//a[contains(@class, 'ProfileHeaderCard-screennameLink u-linkComplex js-nav')]//span[contains(@class, 'username u-dir')]//b");
            var tweets = html.DocumentNode.SelectNodes("//span[contains(@class, 'ProfileNav-value')]");


            Console.WriteLine("\nProfile Description = " + profileDescription.FirstOrDefault().InnerText);
            Console.WriteLine("\nName = " + name.FirstOrDefault().InnerText);
            Console.WriteLine("\nUser(@) = " + user.FirstOrDefault().InnerText);
            Console.WriteLine("");

            string[] a = new string[5];
            int i = 0;
            foreach (var item in tweets)
            {
                a[i] = item.InnerHtml.Replace("\n", "");
                i++;
            }

            Console.WriteLine("Tweets = " + a[0]);
            Console.WriteLine("");
            Console.WriteLine("Seguindo = " + a[1]);
            Console.WriteLine("");
            Console.WriteLine("Seguidores = " + a[2]);
            Console.WriteLine("");
            Console.WriteLine("Curtidas = " + a[3]);

            Select();
        }
    }
}