using System;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Spectre.Console;

public class Program
{
    static public void Main(String[] args)
    {
        // metodin kutsu
        PromilleLaskuri();     
    }

        static void PromilleLaskuri()
        {
            // OTSIKOT
            // Console.WriteLine("Valmiina laskemaan promillet");
            var rule1 = new Rule("[seagreen2] Promillelaskuri [/]");
            // centered teksti
            rule1.Style = Style.Parse("seagreen2 dim");
            AnsiConsole.Write(rule1);

            var ruleD1 = new Rule("[seagreen2] Tämä on suuntaa-antava arvio mahdollisesta promillepitoisuudesta veressäsi. [/]");
            // centered teksti
            ruleD1.Style = Style.Parse("seagreen2 dim");
            AnsiConsole.Write(ruleD1);

            var ruleD2 = new Rule("[seagreen2] Alkoholin palaminen elimistössäsi on yksilöllistä ja siihen vaikuttaa mm. nautittujen annosten määrä sekä aika. [/]");
            // centered teksti
            ruleD2.Style = Style.Parse("seagreen2 dim");
            AnsiConsole.Write(ruleD2);

            double GenderK = 0.0;
            // Valikko, sukupuoli
            var Gender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Anna [green]sukupuolesi[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down)[/]")
                    // .HighlightStyle(Color.Cyan1)
                    .AddChoices(new[] {
                        "Nainen", "Mies",
                    }));

            // Echo the gender back to the terminal
            AnsiConsole.WriteLine($"I agree. Sukupuolesi on {Gender}.");
            Console.WriteLine("");

            if (Gender == "Mies")
            {
                GenderK = 0.75; // kerroin miehelle alkoholin poltossa maksassa
            }
            else if (Gender == "Nainen")
            {
                GenderK = 0.66; // kerroin naiselle alkoholin poltossa maksassa
            }

            // Console.WriteLine("");
            var rule2 = new Rule("[seagreen2]Anna painosi (kg): [/]");
            //rule2.Alignment = Justify.Left;
            rule2.Style = Style.Parse("seagreen2 dim");
            AnsiConsole.Write(rule2);
            double Paino;
            while (!double.TryParse(Console.ReadLine(), out Paino)) Console.WriteLine("Integers only allowed."); // This line will do the trick
            Console.WriteLine($"Painat {Paino} kiloa.");

            // Console.WriteLine("");
            var rule3 = new Rule("[seagreen2]Anna nautittujen alkoholiannoksien määrä: [/]");
            //rule3.Alignment = Justify.Left;
            rule3.Style = Style.Parse("seagreen2 dim");
            AnsiConsole.Write(rule3);
            double AnnosLuku;
            while (!double.TryParse(Console.ReadLine(), out AnnosLuku)) Console.WriteLine("Vain numeroita, kiitos."); // This line will do the trick
            //double AnnosLuku = Convert.ToInt32(Console.ReadLine());

            // Console.WriteLine("");
            var rule4 = new Rule("[seagreen2]Kuinka monta tuntia sitten juomien nautiskelu on aloitettu: [/]");
            //rule4.Alignment = Justify.Left;
            rule4.Style = Style.Parse("seagreen2 dim");
            AnsiConsole.Write(rule4);
            double Aika;
            while (!double.TryParse(Console.ReadLine(), out Aika)) Console.WriteLine("Vain numeroita, kiitos."); // This line will do the trick
            // double Aika = Convert.ToInt32(Console.ReadLine());

            double AnnosTulos = AnnosLuku * 12; // 12 = kerroin 1 alkoholiannokselle eli 12g alkomaholia

            double PainoPoltto = Paino / 10; // ihminen polttaa 1g alkomaholia per 10 painokiloa

            double SukuPkerroin = GenderK * Paino; // huomioidaan sukupuoli kertoimena

            double LoppuTulos = (AnnosTulos - (PainoPoltto * Aika)) / SukuPkerroin;

            double AikaJaljella = (AnnosTulos - (PainoPoltto * Aika)) / PainoPoltto;

            if (LoppuTulos < 0)
            {
                LoppuTulos = 0;
                AikaJaljella = 0;
            }

            LoppuTulos = Math.Round(LoppuTulos * Math.Pow(10, 2)) / Math.Pow(10, 2);

            double roundAikaJaljella = double.Round(AikaJaljella, 0);

            // Synchronous
            AnsiConsole.Status()
                .Start("Aloitan laskemaan...", ctx =>
                {

                    // Simulate some work
                    Thread.Sleep(2500);

                    // Update the status and spinner
                    ctx.Status("Thinking some more..");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    // Simulate some work
                    //AnsiConsole.MarkupLine("Doing some more work...");
                    // Thread.Sleep(2000);

                    AnsiConsole.MarkupLine("Laskelmasi valmistui.");
                    // Create a table
                    var table = new Table();
                    table.Border = TableBorder.Rounded;

                    // Add some columns

                    table.AddColumn("Annoksien määrä").BorderColor(Color.Green3);
                    table.AddColumn(new TableColumn("Juominen aloitettu").Centered());
                    table.AddColumn(new TableColumn("Promillelukemasi").Centered());

                    // Add some rows

                    table.AddRow(new Panel($"[seagreen2]{AnnosLuku} kappaletta.[/]").BorderColor(Color.NavajoWhite1),
                        new Panel("[green]n.[/] " + Aika + " [green]tuntia sitten[/]").BorderColor(Color.NavajoWhite1),
                        new Panel("[yellow]Promillemääräsi on nyt: [/]" + LoppuTulos).BorderColor(Color.NavajoWhite1));

                    table.AddRow(new Panel("[yellow]Sukupuoli: [/]" + Gender).BorderColor(Color.NavajoWhite1),
                        new Panel("Painosi: " + Paino + " KG.").BorderColor(Color.NavajoWhite1),
                        new Panel("Aikaa jäljellä noin: " + roundAikaJaljella + " tuntia nollatulokseen!").BorderColor(Color.NavajoWhite1));

                    // Render the table to the console
                    AnsiConsole.Write(table);

                });

            // string LoppuTulosArvo = "";
            // string LoppuTulosArvo = ToString();

            if (LoppuTulos > 0.5)
            {
                // Create a list of with their colors, values and labels
                var items = new List<(string Label, double Value, Color color)>
            {
                ("Ei Ajokunnossa", LoppuTulos, Color.Red),
            };
                // LoppuTulosArvo = "[red] Ei ajokunnossa! [/]";
                AnsiConsole.Write(new BreakdownChart()
                .Width(30)
                .ShowPercentage()
                .AddItems(items, (item) => new BreakdownChartItem(
                item.Label, item.Value, item.color)));
            }

            else if (LoppuTulos > 0.2 && LoppuTulos < 0.5)
            {
                // Create a list of with their colors, values and labels
                var items = new List<(string Label, double Value, Color color)>
            {
                ("Promillet hieman koholla, kannattaa odottaa hetki", LoppuTulos, Color.Orange3),
            };
                AnsiConsole.Write(new BreakdownChart()
                .Width(70)
                .ShowPercentage()
                .AddItems(items, (item) => new BreakdownChartItem(
                item.Label, item.Value, item.color)));
            }

            else if (LoppuTulos < 0.2)
            {
                // Create a list of with their colors, values and labels
                var items = new List<(string Label, double Value, Color color)>
            {
                ("Ajokunnossa", LoppuTulos, Color.Green),
            };
                //   LoppuTulosArvo = " [green3_1] Ajokunnossa! [/]";
                AnsiConsole.Write(new BreakdownChart()
                .Width(30)
                .ShowPercentage()
                .AddItems(items, (item) => new BreakdownChartItem(
                item.Label, item.Value, item.color)));
            }
        }
    }

