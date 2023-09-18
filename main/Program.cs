using System;
using Newtonsoft.Json.Linq;


namespace main
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isOnline = true;
            var request = new GetRequest("https://www.cbr-xml-daily.ru/daily_json.js");
            request.Run();

            var response = request.Response;
            var json = JObject.Parse(response);

            while (isOnline)
            {
                Console.WriteLine("Введите опцию:\n1 - конвертатор\n2 - Список доступных валют\n" +
                    "3 - выйти\n");
                string user_choice = Console.ReadLine();
                switch (user_choice)
                {
                    case "1":
                        Console.WriteLine("Введите название валюты, которую хотите разменять " +
                            "(необходимо вводить в соответствии с названием валюты в списке валют)");
                        string currencyToExchangeName = Console.ReadLine().ToUpper();
                        if (checkIfExist(json, currencyToExchangeName) == false)
                        {
                            Console.WriteLine("Введена неверная валюта");
                            Console.WriteLine("Нажмите любую кнопку для продолжения...");
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Введите количество валюты");
                            string currencyToExValue = Console.ReadLine();
                            if (float.TryParse(currencyToExValue, out float currencyValue) && (Convert.ToSingle(currencyToExValue) >= 0))
                            {
                                Currency currencyToExchange = new Currency(currencyToExchangeName, currencyValue, getExchangeRate(json, currencyToExchangeName));
                                Console.WriteLine("Введите название первой валюты");
                                string currency1Name = Console.ReadLine().ToUpper();
                                if (checkIfExist(json, currency1Name) == false)
                                {
                                    Console.WriteLine("Введена неверная валюта");
                                    Console.WriteLine("Нажмите любую кнопку для продолжения...");
                                    Console.ReadKey();
                                    continue;
                                }
                                Currency currency1 = new Currency(currency1Name, getExchangeRate(json, currency1Name));
                                Console.WriteLine("Введите название второй валюты или знак '-'");
                                string currency2Name = Console.ReadLine().ToUpper(); 
                                if (checkIfExist(json, currency2Name) == false)
                                {
                                    currencyToExchange.exchange(currency1); 
                                }
                                else 
                                { 
                                    Currency currency2 = new Currency(currency2Name, getExchangeRate(json, currency2Name));
                                    currencyToExchange.exchange(currency1, currency2);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Введена неправильная сумма");
                            }
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                    case "2":
                        showAllCurrencies(json);
                        break;
                    case "3":
                        isOnline = false;
                        break;
                }

                float getExchangeRate(JObject jsObject, string currencyName)
                {
                    if (currencyName != "RUB")
                    {
                        var Valute = jsObject["Valute"];
                        var curName = Valute[currencyName];
                        var curValue = curName["Value"];
                        return (float)curValue;
                    }
                    else
                    {
                        return 1;
                    }
                }

                void showAllCurrencies(JObject jsObject)
                {
                    var Valute = jsObject["Valute"];
                    foreach (var item in Valute)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("Нажмите любую кнопку, чтобы продолжить");
                    Console.ReadKey();
                    Console.Clear();
                }

                bool checkIfExist(JObject jsObject, string currencyName)
                {
                    if (currencyName == "RUB")
                    { return true; }
                    var Valute = jsObject["Valute"];
                    var curName = Valute[currencyName];
                    if (curName == null)
                    {
                        return false;
                    }
                    else return true;
                }

            }
        }
        class Currency
        {
            private string currencyName;
            private float value;
            private float exchangeRate;

            public Currency(string Name, float ExchangeRate)
            {
                currencyName = Name;
                value = 0;
                exchangeRate = ExchangeRate;
            }

            public Currency(string Name, float Value, float ExchangeRate)
            {
                currencyName = Name;
                value = Value;
                exchangeRate = ExchangeRate;
            }

            public void showInfo()
            {
                Console.Write($"{value} {currencyName}\n");
            }
            public void exchange(Currency currency1)
            {
                currency1.value = value * exchangeRate / currency1.exchangeRate;
                Console.WriteLine($"{value} {currencyName} = {currency1.value} {currency1.currencyName}");
            }

            public void exchange(Currency currency1, Currency currency2)
            {
                currency1.value = value * exchangeRate / currency1.exchangeRate;
                currency2.value = value * exchangeRate / currency2.exchangeRate;
                Console.WriteLine($"{value} {currencyName} = {currency1.value} {currency1.currencyName}");
                Console.WriteLine($"{value} {currencyName} = {currency2.value} {currency2.currencyName}");
            }
        }
    }
}
