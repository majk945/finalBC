using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml;

namespace liptak_bc
{
    class Application
    {
        private XmlDocument document { get; set; }
        private List<Product> ProductsList { get; set; }

        public Application()
        {
            ShowBanner();
            this.ProductsList = new List<Product>();
            this.document = new XmlDocument();
            document.Load("C:\\Users\\majk\\Desktop\\111bakalarka\\c-bc-3\\liptak_bc\\liptak_bc\\xmlProducts.xml");
            XmlNodeList products = document.DocumentElement.ChildNodes;
            ParseData(products);
            RunMenu();
        }


        private void ParseData(XmlNodeList Products)
        {
            foreach (XmlNode productNode in Products)
            {
                Product newProduct = new Product();
                newProduct.SetId(int.Parse(productNode["id"]?.InnerText ?? "0"));
                newProduct.SetName(productNode["name"]?.InnerText ?? "");
                newProduct.SetCategory(productNode["category"]?.InnerText ?? "");
                newProduct.SetSubCategory(productNode["subcategory"]?.InnerText ?? "");
                newProduct.SetPrice(double.Parse(productNode["price"]?.InnerText ?? "0"));
                newProduct.SetStock(int.Parse(productNode["stock"]?.InnerText ?? "0"));
                newProduct.SetSold2023(int.Parse(productNode["sold_2023"]?.InnerText ?? "0"));
                newProduct.SetSold2024(int.Parse(productNode["sold_2024"]?.InnerText ?? "0"));
                newProduct.SetSold(int.Parse(productNode["sold"]?.InnerText ?? "0"));

                var AdditionalInfoElement = productNode["additional_information"];
                if (AdditionalInfoElement != null)
                {
                    foreach (XmlNode AdditionalInfo in AdditionalInfoElement)
                    {
                        newProduct.GetAdditionalInfo()[AdditionalInfo.Name] = AdditionalInfo.InnerText;
                    }
                }

                this.ProductsList.Add(newProduct);
            }
        }

        private void SaveData()
        {
            XmlNode root = document.DocumentElement;
            root.RemoveAll();

            foreach (var product in ProductsList)
            {
                XmlElement productElement = document.CreateElement("product");

                XmlElement id = document.CreateElement("id");
                id.InnerText = product.GetId().ToString();
                productElement.AppendChild(id);

                XmlElement name = document.CreateElement("name");
                name.InnerText = product.GetName();
                productElement.AppendChild(name);

                XmlElement category = document.CreateElement("category");
                category.InnerText = product.GetCategory();
                productElement.AppendChild(category);

                XmlElement subcategory = document.CreateElement("subcategory");
                subcategory.InnerText = product.GetSubCategory();
                productElement.AppendChild(subcategory);

                XmlElement price = document.CreateElement("price");
                price.InnerText = product.GetPrice().ToString();
                productElement.AppendChild(price);

                XmlElement stock = document.CreateElement("stock");
                stock.InnerText = product.GetStock().ToString();
                productElement.AppendChild(stock);

                XmlElement sold_2023 = document.CreateElement("sold_2023");
                sold_2023.InnerText = product.GetSold2023().ToString();
                productElement.AppendChild(sold_2023);

                XmlElement sold_2024 = document.CreateElement("sold_2024");
                sold_2024.InnerText = product.GetSold2024().ToString();
                productElement.AppendChild(sold_2024);

                XmlElement sold = document.CreateElement("sold");
                sold.InnerText = product.GetSold().ToString();
                productElement.AppendChild(sold);

                XmlElement additionalInfo = document.CreateElement("additional_information");
                foreach (var info in product.GetAdditionalInfo())
                {
                    XmlElement infoElement = document.CreateElement(info.Key);
                    infoElement.InnerText = info.Value;
                    additionalInfo.AppendChild(infoElement);
                }
                productElement.AppendChild(additionalInfo);

                root.AppendChild(productElement);
            }

            document.Save("C:\\Users\\majk\\Desktop\\liptak_bc\\liptak_bc\\xmlProducts.xml");
        }
        private void RunMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("=====================================");
                Console.WriteLine("           HLAVNÉ MENU             ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=====================================");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 1 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Zobraziť všetky kategórie");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 2 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Zobraziť všetky produkty");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 3 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Hladat produkt");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 4 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Pridať nový produkt");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 5 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Upraviť existujúci produkt");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 6 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Odstrániť produkt");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 7 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Usporiadat produkty");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 8 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("- Ukončiť program");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=====================================");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("     Voľba: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n=====================================");
                Console.ForegroundColor = ConsoleColor.Yellow;
                string choice = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Gray;

                switch (choice)
                {
                    case "1":
                        DisplayCategories();
                        break;
                    case "2":
                        DisplayProducts();
                        break;
                    case "3":
                        SearchProducts();
                        break;
                    case "4":
                        AddNewProduct();
                        break;
                    case "5":
                        UpdateProduct();
                        break;
                    case "6":
                        DeleteProduct();
                        break;
                    case "7":
                        SortProducts();
                        break;
                    case "8":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nĎakujeme za používanie programu. Dovidenia!\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nNeplatná voľba, skúste znova.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\nStlačte ENTER pre pokračovanie...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void AddNewProduct()
        {
            Console.Clear();
            Console.WriteLine("\n===========================");
            Console.WriteLine("        PRIDAŤ NOVÝ PRODUKT      ");
            Console.WriteLine("===========================\n");

            Product newProduct = new Product();

            Console.Write("Zadajte ID: ");
            newProduct.SetId(int.Parse(Console.ReadLine()));

            Console.Write("Zadajte názov: ");
            newProduct.SetName(Console.ReadLine());

            Console.Write("Zadajte kategóriu: ");
            newProduct.SetCategory(Console.ReadLine());

            Console.Write("Zadajte podkategóriu: ");
            newProduct.SetSubCategory(Console.ReadLine());

            Console.Write("Zadajte cenu: ");
            newProduct.SetPrice(double.Parse(Console.ReadLine()));

            Console.Write("Zadajte množstvo na sklade: ");
            newProduct.SetStock(int.Parse(Console.ReadLine()));

            Console.Write("Zadajte počet predaných kusov v roku 2023: ");
            newProduct.SetSold2023(int.Parse(Console.ReadLine()));

            Console.Write("Zadajte počet predaných kusov v roku 2024: ");
            newProduct.SetSold2024(int.Parse(Console.ReadLine()));

            Console.Write("Zadajte celkový počet predaných kusov: ");
            newProduct.SetSold(int.Parse(Console.ReadLine()));

            Console.WriteLine("Zadajte dodatočné informácie (nechajte prázdne pre ukončenie):");
            while (true)
            {
                Console.Write("Zadajte nazov informacie: ");
                string key = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(key)) break;

                Console.Write("Zadajte hodnotu informacie: ");
                string value = Console.ReadLine();
                newProduct.GetAdditionalInfo()[key] = value;

                Console.WriteLine("Chcete pridať ďalšiu dodatočnú informáciu? (ano/nie):");
                string response = Console.ReadLine().Trim().ToLower();
                if (response != "ano") break;
            }

            ProductsList.Add(newProduct);
            SaveData();

            Console.WriteLine("\nProdukt bol úspešne pridaný.");
            Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
            Console.ReadLine();
        }

        private void UpdateProduct()
        {
            Console.Clear();
            Console.WriteLine("\n===========================");
            Console.WriteLine("      UPRAVIŤ EXISTUJÚCI PRODUKT    ");
            Console.WriteLine("===========================\n");

            Console.Write("Zadajte ID produktu, ktorý chcete upraviť: ");
            int productId = int.Parse(Console.ReadLine());

            Product product = ProductsList.FirstOrDefault(p => p.GetId() == productId);
            if (product == null)
            {
                Console.WriteLine("\nProdukt s týmto ID neexistuje.");
                Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
                Console.ReadLine();
                return;
            }

            Console.Write("Zadajte nový názov (aktuálny: {0}): ", product.GetName());
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) product.SetName(newName);

            Console.Write("Zadajte novú kategóriu (aktuálna: {0}): ", product.GetCategory());
            string newCategory = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCategory)) product.SetCategory(newCategory);

            Console.Write("Zadajte novú podkategóriu (aktuálna: {0}): ", product.GetSubCategory());
            string newSubCategory = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newSubCategory)) product.SetSubCategory(newSubCategory);

            Console.Write("Zadajte novú cenu (aktuálna: {0}): ", product.GetPrice());
            string newPrice = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPrice)) product.SetPrice(double.Parse(newPrice));

            Console.Write("Zadajte nové množstvo na sklade (aktuálne: {0}): ", product.GetStock());
            string newStock = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newStock)) product.SetStock(int.Parse(newStock));

            Console.Write("Zadajte nový počet predaných kusov v roku 2023 (aktuálne: {0}): ", product.GetSold2023());
            string newSold2023 = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newSold2023)) product.SetSold2023(int.Parse(newSold2023));

            Console.Write("Zadajte nový počet predaných kusov v roku 2024 (aktuálne: {0}): ", product.GetSold2024());
            string newSold2024 = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newSold2024)) product.SetSold2024(int.Parse(newSold2024));

            Console.Write("Zadajte nový celkový počet predaných kusov (aktuálne: {0}): ", product.GetSold());
            string newSold = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newSold)) product.SetSold(int.Parse(newSold));

            Console.WriteLine("Zadajte nové dodatočné informácie (nechajte prázdne pre ukončenie):");
            while (true)
            {
                Console.Write("Zadajte nazov informacie: ");
                string key = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(key)) break;

                Console.Write("Zadajte hodnotu informacie: ");
                string value = Console.ReadLine();
                product.GetAdditionalInfo()[key] = value;

                Console.WriteLine("Chcete pridať ďalšiu dodatočnú informáciu? (ano/nie):");
                string response = Console.ReadLine().Trim().ToLower();
                if (response != "ano") break;
            }

            SaveData();

            Console.WriteLine("\nProdukt bol úspešne aktualizovaný.");
            Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
            Console.ReadLine();
        }
        private void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("\n===========================");
            Console.WriteLine("        ODSTRÁNIŤ PRODUKT        ");
            Console.WriteLine("===========================\n");

            Console.Write("Zadajte ID produktu, ktorý chcete odstrániť: ");
            int productId = int.Parse(Console.ReadLine());

            Product product = ProductsList.FirstOrDefault(p => p.GetId() == productId);
            if (product == null)
            {
                Console.WriteLine("\nProdukt s týmto ID neexistuje.");
                Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
                Console.ReadLine();
                return;
            }

            ProductsList.Remove(product);
            SaveData();

            Console.WriteLine("\nProdukt bol úspešne odstránený.");
            Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
            Console.ReadLine();
        }

        private void SortProducts()
        {
            Console.Clear();
            Console.WriteLine("\n===========================");
            Console.WriteLine("        USPORIADAT PRODUKTY        ");
            Console.WriteLine("===========================\n");

            Console.WriteLine("Chcete triediť zo všetkých produktov, špecifických produktov, alebo podľa dodatočných informácií?");
            Console.WriteLine("1. Všetky produkty");
            Console.WriteLine("2. Špecifické produkty");
            Console.WriteLine("3. Podľa dodatočných informácií");
            Console.Write("\nVyberte možnosť (1-3): ");
            string sortChoice = Console.ReadLine();

            List<Product> productsToSort;

            if (sortChoice == "2")
            {
                Console.WriteLine("Najprv vyhľadajte produkty podľa kritérií.");
                var filters = GetSearchFilters();
                productsToSort = FilterProducts(ProductsList, filters);

                if (productsToSort.Count == 0)
                {
                    Console.WriteLine("\nŽiadne produkty nevyhovujú zadaným kritériám.");
                    Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
                    Console.ReadLine();
                    return;
                }

                DisplaySearchResults(productsToSort);

                // Adding additional sorting based on additional information
                Console.WriteLine("\nZvoľte kritériá triedenia podľa dodatočných informácií:");
                var sampleProduct = productsToSort.FirstOrDefault();
                if (sampleProduct != null)
                {
                    foreach (var info in sampleProduct.GetAdditionalInfo().Keys)
                    {
                        Console.WriteLine($"Kritérium: {info}");
                    }
                }

                Console.WriteLine("Zadajte názov kritéria (napr. Lumen):");
                string criterionKey = Console.ReadLine().Trim();

                if (sampleProduct != null && sampleProduct.GetAdditionalInfo().ContainsKey(criterionKey))
                {
                    bool ascending = true;
                    Console.WriteLine("Chcete triediť vzostupne (v) alebo zostupne (z)?");
                    string orderChoice = Console.ReadLine().Trim().ToLower();
                    ascending = orderChoice == "v";

                    Comparison<Product> comparison = (p1, p2) =>
                    {
                        if (p1.GetAdditionalInfo().ContainsKey(criterionKey) && p2.GetAdditionalInfo().ContainsKey(criterionKey))
                        {
                            string val1 = p1.GetAdditionalInfo()[criterionKey];
                            string val2 = p2.GetAdditionalInfo()[criterionKey];

                            int result;
                            if (double.TryParse(val1, out double num1) && double.TryParse(val2, out double num2))
                            {
                                result = num1.CompareTo(num2);
                            }
                            else
                            {
                                result = string.Compare(val1, val2, StringComparison.OrdinalIgnoreCase);
                            }

                            return ascending ? result : -result;
                        }
                        return 0;
                    };

                    InsertionSort(productsToSort, comparison);
                }
                else
                {
                    Console.WriteLine("\nKritérium nebolo nájdené, skúste znova.");
                    Console.WriteLine("\nStlačte ENTER pre pokračovanie");
                    Console.ReadLine();
                    return;
                }
            }
            else if (sortChoice == "3")
            {
                var filters = GetAdditionalSearchFilters(false);
                productsToSort = FilterProductsByAdditionalInfo(ProductsList, filters);

                if (productsToSort.Count == 0)
                {
                    Console.WriteLine("\nŽiadne produkty nevyhovujú zadaným kritériám.");
                    Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
                    Console.ReadLine();
                    return;
                }

                DisplaySearchResults(productsToSort);

                Console.WriteLine("\nZvoľte kritériá triedenia podľa dodatočných informácií:");
                var sampleProduct = productsToSort.FirstOrDefault();
                if (sampleProduct != null)
                {
                    foreach (var info in sampleProduct.GetAdditionalInfo().Keys)
                    {
                        Console.WriteLine($"Kritérium: {info}");
                    }
                }
                Console.WriteLine("Zadajte názov kritéria (napr. Lumen):");
                string criterionKey = Console.ReadLine().Trim();

                if (sampleProduct != null && sampleProduct.GetAdditionalInfo().ContainsKey(criterionKey))
                {
                    string val = sampleProduct.GetAdditionalInfo()[criterionKey];
                    bool ascending = true;
                    if (double.TryParse(val, out _))
                    {
                        Console.WriteLine("Chcete triediť vzostupne (v) alebo zostupne (z)?");
                        string orderChoice = Console.ReadLine().Trim().ToLower();
                        ascending = orderChoice == "v";
                    }
                    else
                    {
                        Console.WriteLine("Chcete triediť a-z alebo z-a? (1/0)");
                        string orderChoice = Console.ReadLine().Trim().ToLower();
                        ascending = orderChoice == "1";
                    }

                    Comparison<Product> comparison = (p1, p2) =>
                    {
                        if (p1.GetAdditionalInfo().ContainsKey(criterionKey) && p2.GetAdditionalInfo().ContainsKey(criterionKey))
                        {
                            string val1 = p1.GetAdditionalInfo()[criterionKey];
                            string val2 = p2.GetAdditionalInfo()[criterionKey];

                            int result;
                            if (double.TryParse(val1, out double num1) && double.TryParse(val2, out double num2))
                            {
                                result = num1.CompareTo(num2);
                            }
                            else
                            {
                                result = string.Compare(val1, val2, StringComparison.OrdinalIgnoreCase);
                            }

                            return ascending ? result : -result;
                        }
                        return 0;
                    };

                    InsertionSort(productsToSort, comparison);
                }
                else
                {
                    Console.WriteLine("\nKritérium nebolo nájdené, skúste znova.");
                    Console.WriteLine("\nStlačte ENTER pre pokračovanie");
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                productsToSort = new List<Product>(ProductsList);

                Console.WriteLine("\nZvoľte kritériá triedenia:");
                Console.WriteLine("1. Podľa ID (vzostupne/zostupne)");
                Console.WriteLine("2. Podľa názvu (vzostupne/zostupne)");
                Console.WriteLine("3. Podľa kategórie (vzostupne/zostupne)");
                Console.WriteLine("4. Podľa podkategórie (vzostupne/zostupne)");
                Console.WriteLine("5. Podľa ceny (vzostupne/zostupne)");
                Console.WriteLine("6. Podľa množstva na sklade (vzostupne/zostupne)");
                Console.WriteLine("9. Podľa predaného množstva v roku 2023 (vzostupne/zostupne)");
                Console.WriteLine("10. Podľa predaného množstva v roku 2024 (vzostupne/zostupne)");
                Console.WriteLine("11. Podľa celkového predaného množstva (vzostupne/zostupne)");
                Console.WriteLine("Zadajte cislo a hodnotu (napr. 1z,3v,5z):");

                string choice = Console.ReadLine();
                var criteria = choice.Split(',').Select(c => c.Trim()).ToList();

                Comparison<Product> comparison = (p1, p2) =>
                {
                    foreach (var criterion in criteria)
                    {
                        string criterionKey = criterion.Substring(0, criterion.Length - 1);
                        bool ascending = criterion.EndsWith("v", StringComparison.OrdinalIgnoreCase);
                        int result = 0;

                        switch (criterionKey)
                        {
                            case "1":
                                result = p1.GetId().CompareTo(p2.GetId());
                                break;
                            case "2":
                                result = p1.GetName().CompareTo(p2.GetName());
                                break;
                            case "3":
                                result = p1.GetCategory().CompareTo(p2.GetCategory());
                                break;
                            case "4":
                                result = p1.GetSubCategory().CompareTo(p2.GetSubCategory());
                                break;
                            case "5":
                                result = p1.GetPrice().CompareTo(p2.GetPrice());
                                break;
                            case "6":
                                result = p1.GetStock().CompareTo(p2.GetStock());
                                break;
                            case "9":
                                result = p1.GetSold2023().CompareTo(p2.GetSold2023());
                                break;
                            case "10":
                                result = p1.GetSold2024().CompareTo(p2.GetSold2024());
                                break;
                            case "11":
                                result = p1.GetSold().CompareTo(p2.GetSold());
                                break;
                            default:
                                Console.WriteLine("\nNeplatná voľba, skúste znova.");
                                Console.WriteLine("\nStlačte ENTER pre pokračovanie");
                                Console.ReadLine();
                                return 0;
                        }

                        if (result != 0)
                            return ascending ? result : -result;
                    }
                    return 0;
                };

                InsertionSort(productsToSort, comparison);
            }

            // Display sorted results
            Console.Clear();
            Console.WriteLine("\n===========================");
            Console.WriteLine("        VÝSLEDOK TRIEDENIA        ");
            Console.WriteLine("===========================\n");

            DisplaySortedProducts(productsToSort);

            Console.WriteLine("\nChcete uložiť usporiadane výsledky? (ano/nie):");
            string saveResponse = Console.ReadLine().Trim().ToLower();
            if (saveResponse == "ano")
            {
                SaveSortedResults(productsToSort);
            }
        }

        private SearchFilters GetAdditionalSearchFilters(bool promptForAdditionalInfo = true)
        {
            var filters = new SearchFilters();

            Console.Write("Zadajte kategóriu: ");
            filters.CategoryFilter = Console.ReadLine()?.Trim().ToLower() ?? "";

            Console.Write("Chcete zadať aj podkategóriu? (ano/nie): ");
            string includeSubCategory = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (includeSubCategory == "ano")
            {
                Console.Write("Zadajte podkategóriu: ");
                filters.SubCategoryFilter = Console.ReadLine()?.Trim().ToLower() ?? "";
            }

            if (promptForAdditionalInfo)
            {
                Console.WriteLine("\nDostupné dodatočné informácie pre kategóriu '{0}':", filters.CategoryFilter);
                var sampleProduct = ProductsList.FirstOrDefault(p => p.GetCategory().ToLower() == filters.CategoryFilter);
                if (sampleProduct != null)
                {
                    foreach (var info in sampleProduct.GetAdditionalInfo().Keys)
                    {
                        Console.WriteLine($"- {info}");
                    }
                }

                Console.WriteLine("\nZadajte dodatočné informácie (nechajte prázdne pre ukončenie):");
                while (true)
                {
                    Console.Write("Zadajte nazov informacie: ");
                    string key = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(key)) break;

                    Console.Write("Zadajte hodnotu informacie: ");
                    string value = Console.ReadLine();
                    filters.AdditionalInfoFilters[key.ToLower()] = value.ToLower();
                }
            }

            return filters;
        }


        private bool TryExtractNumber(string input, out double number)
        {
            number = 0;
            var match = Regex.Match(input, @"\d+");
            if (match.Success)
            {
                return double.TryParse(match.Value, out number);
            }
            return false;
        }
        private int CompareEnergyClass(string class1, string class2)
        {
            string[] energyClasses = { "A", "B", "C", "D", "E", "F", "G" };
            int index1 = Array.IndexOf(energyClasses, class1.ToUpper());
            int index2 = Array.IndexOf(energyClasses, class2.ToUpper());

            return index1.CompareTo(index2);
        }


        private void DisplaySortedProducts(List<Product> sortedProducts)
        {
            Console.Clear();
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("                    ZOZNAM USPORIADANYCH PRODUKTOV                ");
            Console.WriteLine("==============================================================");

            Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8} | {5,-6} | {6,-10} | {7,-10} | {8,-5}",
                "ID", "Názov", "Kategória", "Podkategória", "Cena (EUR)", "Sklad", "Predané 2023", "Predané 2024", "Celkom predané");
            Console.WriteLine(new string('=', 110));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8:F2} | {5,-6} ks | {6,-10} | {7,-10} | {8,-5}",
                    product.GetId(),
                    product.GetName().Length > 35 ? product.GetName().Substring(0, 32) + "..." : product.GetName(),
                    product.GetCategory(),
                    product.GetSubCategory(),
                    product.GetPrice(),
                    product.GetStock(),
                    product.GetSold2023(),
                    product.GetSold2024(),
                    product.GetSold());

                if (product.GetAdditionalInfo().Count > 0)
                {
                    Console.WriteLine("\n   Dodatočné informácie:");
                    foreach (var info in product.GetAdditionalInfo())
                    {
                        Console.WriteLine("     - {0,-15}: {1}", info.Key, info.Value);
                    }
                }

                Console.WriteLine(new string('-', 110));
            }

            Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
            Console.ReadLine();
        }
        private void DisplayCategories()
        {
            HashSet<string> categories = new HashSet<string>();

            foreach (var product in ProductsList)
            {
                categories.Add(product.GetCategory());
            }

            Console.WriteLine("\nDostupné kategórie:");
            foreach (var category in categories)
            {
                Console.WriteLine($"- {category}");
            }

            while (true)
            {
                Console.WriteLine("\nVyberte kategóriu alebo napíšte 'spat' pre návrat: ");
                string selectedCategory = Console.ReadLine();

                if (selectedCategory.ToLower() == "spat")
                {
                    return;
                }
                else if (categories.Contains(selectedCategory))
                {
                    Console.WriteLine("\nChcete zobraziť:");
                    Console.WriteLine("1 - Všetky produkty v tejto kategórii");
                    Console.WriteLine("2 - Podkategórie tejto kategórie");
                    Console.Write("Voľba: ");

                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        DisplayProductsByCategory(selectedCategory);
                        return;
                    }
                    else if (choice == "2")
                    {
                        DisplaySubCategoryByCategory(selectedCategory);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Neplatná voľba, skúste znova.");
                    }
                }
                else
                {
                    Console.WriteLine("Neplatná kategória, skúste znova.");
                }
            }
        }

        private void DisplayProductsByCategory(string category)
        {
            Console.Clear();
            Console.WriteLine($"\n==============================================================");
            Console.WriteLine($"            PRODUKTY V KATEGÓRII: {category.ToUpper()}");
            Console.WriteLine($"==============================================================");

            Console.WriteLine("{0,-5} | {1,-35} | {2,-20} | {3,-8} | {4,-6} | {5,-10} | {6,-10} | {7,-5}",
                "ID", "Názov", "Podkategória", "Cena (EUR)", "Sklad", "Predané 2023", "Predané 2024", "Celkom predané");
            Console.WriteLine(new string('=', 110));

            bool found = false;
            foreach (var product in ProductsList)
            {
                if (product.GetCategory().Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("{0,-5} | {1,-35} | {2,-20} | {3,-8:F2} | {4,-6} ks | {5,-10} | {6,-10} | {7,-5}",
                        product.GetId(),
                        product.GetName().Length > 35 ? product.GetName().Substring(0, 32) + "..." : product.GetName(),
                        product.GetSubCategory(),
                        product.GetPrice(),
                        product.GetStock(),
                        product.GetSold2023(),
                        product.GetSold2024(),
                        product.GetSold());

                    if (product.GetAdditionalInfo().Count > 0)
                    {
                        Console.WriteLine("\n   Dodatočné informácie:");
                        foreach (var info in product.GetAdditionalInfo())
                        {
                            Console.WriteLine("     - {0,-15}: {1}", info.Key, info.Value);
                        }
                    }

                    Console.WriteLine(new string('-', 110));
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("V tejto kategórii sa nenachádzajú žiadne produkty.");
            }

            Console.WriteLine("\nStlačte ENTER pre pokračovanie");
            Console.ReadLine();
        }

        private void DisplaySubCategoryByCategory(string category)
        {
            Console.Clear();
            Console.WriteLine($"\n==============================================================");
            Console.WriteLine($"          PODKATEGÓRIE V KATEGÓRII: {category.ToUpper()}");
            Console.WriteLine($"==============================================================");

            HashSet<string> subCategories = new HashSet<string>();

            foreach (var product in ProductsList)
            {
                if (product.GetCategory().Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    subCategories.Add(product.GetSubCategory());
                }
            }

            if (subCategories.Count == 0)
            {
                Console.WriteLine("V tejto kategórii sa nenachádzajú žiadne podkategórie.");
                Console.WriteLine("\nStlačte ENTER pre pokračovanie");
                Console.ReadLine();
                return;
            }

            int index = 1;
            Dictionary<int, string> selectionMap = new Dictionary<int, string>();
            foreach (var subCategory in subCategories)
            {
                Console.WriteLine($"{index}. {subCategory}");
                selectionMap[index] = subCategory;
                index++;
            }

            Console.WriteLine("\nZadajte číslo podkategórie pre zobrazenie jej produktov (alebo 0 pre návrat):");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > selectionMap.Count)
            {
                Console.WriteLine("Neplatná voľba, skúste znova:");
            }

            if (choice == 0)
            {
                return;
            }

            DisplayProductBySubCategory(selectionMap[choice]);
        }

        private void DisplayProductBySubCategory(string subCategory)
        {
            Console.Clear();
            Console.WriteLine($"\n==============================================================");
            Console.WriteLine($"        PRODUKTY V PODKATEGÓRII: {subCategory.ToUpper()}");
            Console.WriteLine($"==============================================================");

            Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-8} | {4,-6}",
                "ID", "Názov", "Kategória", "Cena (EUR)", "Sklad");
            Console.WriteLine(new string('=', 80));

            bool found = false;
            foreach (var product in ProductsList)
            {
                if (product.GetSubCategory().Equals(subCategory, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-8:F2} | {4,-6} ks",
                    product.GetId(),
                    product.GetName().Length > 35 ? product.GetName().Substring(0, 32) + "..." : product.GetName(),
                    product.GetCategory(),
                    product.GetPrice(),
                    product.GetStock());

                    if (product.GetAdditionalInfo().Count > 0)
                    {
                        Console.WriteLine("\n   Dodatočné informácie:");
                        foreach (var info in product.GetAdditionalInfo())
                        {
                            Console.WriteLine("     - {0,-15}: {1}", info.Key, info.Value);
                        }
                    }

                    Console.WriteLine(new string('-', 80));
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("V tejto podkategórii sa nenachádzajú žiadne produkty.");
            }

            Console.WriteLine("\nStlačte ENTER pre pokračovanie");
            Console.ReadLine();
        }

        private void DisplaySortedProducts()
        {
            Console.Clear();
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("                    ZOZNAM USPORIADANYCH PRODUKTOV                ");
            Console.WriteLine("==============================================================");

            Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8} | {5,-6}",
                "ID", "Názov", "Kategória", "Podkategória", "Cena (EUR)", "Sklad");
            Console.WriteLine(new string('=', 90));

            foreach (var product in ProductsList)
            {
                Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8:F2} | {5,-6} ks",
                    product.GetId(),
                    product.GetName().Length > 35 ? product.GetName().Substring(0, 32) + "..." : product.GetName(),
                    product.GetCategory(),
                    product.GetSubCategory(),
                    product.GetPrice(),
                    product.GetStock());

                if (product.GetAdditionalInfo().Count > 0)
                {
                    Console.WriteLine("\n   Dodatočné informácie:");
                    foreach (var info in product.GetAdditionalInfo())
                    {
                        Console.WriteLine("     - {0,-15}: {1}", info.Key, info.Value);
                    }
                }

                Console.WriteLine(new string('-', 90));
            }

            Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
            Console.ReadLine();
        }

        private void SaveSortedResults(List<Product> sortedProducts)
        {
            string date = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string fileName = $"UsporiadaneProdukty_{date}.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("==============================================================");
                writer.WriteLine("                    ZOZNAM USPORIADANYCH PRODUKTOV                ");
                writer.WriteLine("==============================================================");

                writer.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8} | {5,-6}",
                    "ID", "Názov", "Kategória", "Podkategória", "Cena (EUR)", "Sklad");
                writer.WriteLine(new string('=', 90));

                foreach (var product in sortedProducts)
                {
                    writer.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8:F2} | {5,-6} ks",
                        product.GetId(),
                        product.GetName().Length > 35 ? product.GetName().Substring(0, 32) + "..." : product.GetName(),
                        product.GetCategory(),
                        product.GetSubCategory(),
                        product.GetPrice(),
                        product.GetStock());

                    if (product.GetAdditionalInfo().Count > 0)
                    {
                        writer.WriteLine("\n   Dodatočné informácie:");
                        foreach (var info in product.GetAdditionalInfo())
                        {
                            writer.WriteLine("     - {0,-15}: {1}", info.Key, info.Value);
                        }
                    }

                    writer.WriteLine(new string('-', 90));
                }
            }

            Console.WriteLine($"\n✅ Usporiadane výsledky boli uložené do súboru: {fileName}");
            Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
            Console.ReadLine();
        }
        private void DisplayProducts()
        {
            Console.Clear();
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("                    ZOZNAM VŠETKÝCH PRODUKTOV                ");
            Console.WriteLine("==============================================================");

            Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8} | {5,-6} | {6,-10} | {7,-10} | {8,-5}",
                "ID", "Názov", "Kategória", "Podkategória", "Cena (EUR)", "Sklad", "Predané 2023", "Predané 2024", "Celkom predané");
            Console.WriteLine(new string('=', 110));

            foreach (var product in ProductsList)
            {
                Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-8:F2} | {5,-6} ks | {6,-10} | {7,-10} | {8,-5}",
                    product.GetId(),
                    product.GetName().Length > 35 ? product.GetName().Substring(0, 32) + "..." : product.GetName(),
                    product.GetCategory(),
                    product.GetSubCategory(),
                    product.GetPrice(),
                    product.GetStock(),
                    product.GetSold2023(),
                    product.GetSold2024(),
                    product.GetSold());

                if (product.GetAdditionalInfo().Count > 0)
                {
                    Console.WriteLine("\n   Dodatočné informácie:");
                    foreach (var info in product.GetAdditionalInfo())
                    {
                        Console.WriteLine("     - {0,-15}: {1}", info.Key, info.Value);
                    }
                }

                Console.WriteLine(new string('-', 110));
            }

            Console.WriteLine("\nStlačte ENTER pre návrat do hlavného menu...");
            Console.ReadLine();
        }
        private void SearchProducts()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n===========================");
                Console.WriteLine("        HĽADANIE PRODUKTOV        ");
                Console.WriteLine("===========================\n");

                Console.WriteLine("Chcete vykonať základné hľadanie alebo hľadanie podľa dodatočných informácií?");
                Console.WriteLine("1. Základné hľadanie");
                Console.WriteLine("2. Hľadanie podľa dodatočných informácií");
                Console.Write("\nVyberte možnosť (1-2): ");
                string searchChoice = Console.ReadLine();

                if (searchChoice == "2")
                {
                    var filters = GetAdditionalSearchFilters();
                    var filteredProducts = FilterProductsByAdditionalInfo(ProductsList, filters);
                    DisplaySearchResults(filteredProducts);
                }
                else
                {
                    var filters = GetSearchFilters();
                    var filteredProducts = FilterProducts(ProductsList, filters);
                    DisplaySearchResults(filteredProducts);
                }

                Console.WriteLine("\nMožnosti:");
                Console.WriteLine("1. Nové vyhľadávanie");
                Console.WriteLine("2. Návrat do hlavného menu");
                Console.Write("\nVyberte možnosť (1-2): ");

                string choice = Console.ReadLine();
                if (choice != "1")
                    break;
            }
        }
        private SearchFilters GetAdditionalSearchFilters()
        {
            var filters = new SearchFilters();

            Console.Write("Zadajte kategóriu: ");
            filters.CategoryFilter = Console.ReadLine()?.Trim().ToLower() ?? "";

            Console.Write("Chcete zadať aj podkategóriu? (ano/nie): ");
            string includeSubCategory = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (includeSubCategory == "ano")
            {
                Console.Write("Zadajte podkategóriu: ");
                filters.SubCategoryFilter = Console.ReadLine()?.Trim().ToLower() ?? "";
            }

            Console.WriteLine("\nDostupné dodatočné informácie pre kategóriu '{0}':", filters.CategoryFilter);
            var sampleProduct = ProductsList.FirstOrDefault(p => p.GetCategory().ToLower() == filters.CategoryFilter);
            if (sampleProduct != null)
            {
                foreach (var info in sampleProduct.GetAdditionalInfo().Keys)
                {
                    Console.WriteLine($"- {info}");
                }
            }

            Console.WriteLine("\nZadajte dodatočné informácie (nechajte prázdne pre ukončenie):");
            while (true)
            {
                Console.Write("Zadajte nazov informacie: ");
                string key = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(key)) break;

                Console.Write("Zadajte hodnotu informacie: ");
                string value = Console.ReadLine();
                filters.AdditionalInfoFilters[key.ToLower()] = value.ToLower();
            }

            return filters;
        }
        private List<Product> FilterProductsByAdditionalInfo(List<Product> products, SearchFilters filters)
        {
            return products.Where(product =>
                (string.IsNullOrWhiteSpace(filters.CategoryFilter) || product.GetCategory().ToLower().Equals(filters.CategoryFilter)) &&
                (string.IsNullOrWhiteSpace(filters.SubCategoryFilter) || product.GetSubCategory().ToLower().Equals(filters.SubCategoryFilter)) &&
                filters.AdditionalInfoFilters.All(filter =>
                    product.GetAdditionalInfo().ContainsKey(filter.Key) &&
                    product.GetAdditionalInfo()[filter.Key].ToLower().Contains(filter.Value)
                )
            ).ToList();
        }


        private SearchFilters GetSearchFilters()
        {
            var filters = new SearchFilters();

            Console.Write("Zadajte názov produktu (nechajte prázdne pre ignorovanie): ");
            filters.NameFilter = Console.ReadLine()?.Trim().ToLower() ?? "";

            Console.Write("Zadajte kategóriu (nechajte prázdne pre ignorovanie): ");
            filters.CategoryFilter = Console.ReadLine()?.Trim().ToLower() ?? "";

            Console.Write("Zadajte podkategóriu (nechajte prázdne pre ignorovanie): ");
            filters.SubCategoryFilter = Console.ReadLine()?.Trim().ToLower() ?? "";

            Console.Write("Zadajte minimálnu cenu (nechajte prázdne pre ignorovanie): ");
            string minPriceInput = Console.ReadLine()?.Trim() ?? "";
            filters.MinPrice = string.IsNullOrWhiteSpace(minPriceInput) ? 0 : double.TryParse(minPriceInput, out double minPrice) ? minPrice : 0;

            Console.Write("Zadajte maximálnu cenu (nechajte prázdne pre ignorovanie): ");
            string maxPriceInput = Console.ReadLine()?.Trim() ?? "";
            filters.MaxPrice = string.IsNullOrWhiteSpace(maxPriceInput) ? double.MaxValue : double.TryParse(maxPriceInput, out double maxPrice) ? maxPrice : double.MaxValue;

            Console.Write("Zadajte minimálny počet predaných kusov v roku 2023 (nechajte prázdne pre ignorovanie): ");
            string minSold2023Input = Console.ReadLine()?.Trim() ?? "";
            filters.MinSold2023 = string.IsNullOrWhiteSpace(minSold2023Input) ? 0 : int.TryParse(minSold2023Input, out int minSold2023) ? minSold2023 : 0;

            Console.Write("Zadajte maximálny počet predaných kusov v roku 2023 (nechajte prázdne pre ignorovanie): ");
            string maxSold2023Input = Console.ReadLine()?.Trim() ?? "";
            filters.MaxSold2023 = string.IsNullOrWhiteSpace(maxSold2023Input) ? int.MaxValue : int.TryParse(maxSold2023Input, out int maxSold2023) ? maxSold2023 : int.MaxValue;

            Console.Write("Zadajte minimálny počet predaných kusov v roku 2024 (nechajte prázdne pre ignorovanie): ");
            string minSold2024Input = Console.ReadLine()?.Trim() ?? "";
            filters.MinSold2024 = string.IsNullOrWhiteSpace(minSold2024Input) ? 0 : int.TryParse(minSold2024Input, out int minSold2024) ? minSold2024 : 0;

            Console.Write("Zadajte maximálny počet predaných kusov v roku 2024 (nechajte prázdne pre ignorovanie): ");
            string maxSold2024Input = Console.ReadLine()?.Trim() ?? "";
            filters.MaxSold2024 = string.IsNullOrWhiteSpace(maxSold2024Input) ? int.MaxValue : int.TryParse(maxSold2024Input, out int maxSold2024) ? maxSold2024 : int.MaxValue;

            Console.Write("Zadajte minimálny celkový počet predaných kusov (nechajte prázdne pre ignorovanie): ");
            string minSoldInput = Console.ReadLine()?.Trim() ?? "";
            filters.MinSold = string.IsNullOrWhiteSpace(minSoldInput) ? 0 : int.TryParse(minSoldInput, out int minSold) ? minSold : 0;

            Console.Write("Zadajte maximálny celkový počet predaných kusov (nechajte prázdne pre ignorovanie): ");
            string maxSoldInput = Console.ReadLine()?.Trim() ?? "";
            filters.MaxSold = string.IsNullOrWhiteSpace(maxSoldInput) ? int.MaxValue : int.TryParse(maxSoldInput, out int maxSold) ? maxSold : int.MaxValue;

            return filters;
        }
        private List<Product> FilterProducts(List<Product> products, SearchFilters filters)
        {
            return products.Where(product =>
                (string.IsNullOrWhiteSpace(filters.NameFilter) || product.GetName().ToLower().Contains(filters.NameFilter)) &&
                (string.IsNullOrWhiteSpace(filters.CategoryFilter) || product.GetCategory().ToLower().Equals(filters.CategoryFilter)) &&
                (string.IsNullOrWhiteSpace(filters.SubCategoryFilter) || product.GetSubCategory().ToLower().Equals(filters.SubCategoryFilter)) &&
                (product.GetPrice() >= filters.MinPrice && product.GetPrice() <= filters.MaxPrice) &&
                (!filters.FilterInStock || product.GetStock() > 0) &&
                (product.GetSold2023() >= filters.MinSold2023 && product.GetSold2023() <= filters.MaxSold2023) &&
                (product.GetSold2024() >= filters.MinSold2024 && product.GetSold2024() <= filters.MaxSold2024) &&
                (product.GetSold() >= filters.MinSold && product.GetSold() <= filters.MaxSold)
            ).ToList();
        }
        private void DisplaySearchResults(List<Product> filteredProducts)
        {
            Console.Clear();
            Console.WriteLine("\n===========================");
            Console.WriteLine("        VÝSLEDKY HĽADANIA       ");
            Console.WriteLine("===========================\n");

            if (filteredProducts.Count == 0)
            {
                Console.WriteLine("Žiadne produkty nevyhovujú zadaným kritériám.");
                return;
            }

            Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-10} | {5,-8} | {6,-10} | {7,-10} | {8,-5}",
                "ID", "Názov", "Kategória", "Podkategória", "Cena (€)", "Množstvo", "Predané 2023", "Predané 2024", "Celkom predané");
            Console.WriteLine(new string('-', 120));

            foreach (var product in filteredProducts)
            {
                Console.WriteLine("{0,-5} | {1,-35} | {2,-15} | {3,-20} | {4,-10:F2} | {5,-8} ks | {6,-10} | {7,-10} | {8,-5}",
                    product.GetId(),
                    TruncateString(product.GetName(), 35),
                    product.GetCategory(),
                    product.GetSubCategory(),
                    product.GetPrice(),
                    product.GetStock(),
                    product.GetSold2023(),
                    product.GetSold2024(),
                    product.GetSold());

                if (product.GetAdditionalInfo().Count > 0)
                {
                    Console.WriteLine("\n   Dodatočné informácie:");
                    foreach (var info in product.GetAdditionalInfo())
                    {
                        Console.WriteLine("     - {0,-15}: {1}", info.Key, info.Value);
                    }
                }

                Console.WriteLine(new string('-', 120));
            }

            Console.WriteLine($"\nNájdených produktov: {filteredProducts.Count}");
            Console.WriteLine("\nStlačte ENTER pre pokračovanie");
            Console.ReadLine();
        }
        private void InsertionSort<T>(List<T> list, Comparison<T> comparison)
        {
            for (int i = 1; i < list.Count; i++)
            {
                T key = list[i];
                int j = i - 1;

                while (j >= 0 && comparison(list[j], key) > 0)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }

        private class SearchFilters
        {
            public string NameFilter { get; set; } = "";
            public string CategoryFilter { get; set; } = "";
            public string SubCategoryFilter { get; set; } = "";
            public double MinPrice { get; set; } = 0;
            public double MaxPrice { get; set; } = double.MaxValue;
            public bool FilterInStock { get; set; } = false;
            public int MinSold2023 { get; set; } = 0;
            public int MaxSold2023 { get; set; } = int.MaxValue;
            public int MinSold2024 { get; set; } = 0;
            public int MaxSold2024 { get; set; } = int.MaxValue;
            public int MinSold { get; set; } = 0;
            public int MaxSold { get; set; } = int.MaxValue;
            public Dictionary<string, string> AdditionalInfoFilters { get; set; } = new Dictionary<string, string>();
        }

        private string TruncateString(string str, int maxLength)
        {
            return str.Length > maxLength ? str.Substring(0, maxLength - 3) + "..." : str;
        }

        private void ShowBanner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;


            Console.WriteLine(@" _____ _      _    _             _   _       _     
| ____| | ___| | _| |_ _ __ ___ | | | |_   _| |__  
|  _| | |/ _ \ |/ / __| '__/ _ \| |_| | | | | '_ \ 
| |___| |  __/   <| |_| | | (_) |  _  | |_| | |_) |
|_____|_|\___|_|\_\\__|_|  \___/|_| |_|\__,_|_.__/ ");

            Console.ForegroundColor = ConsoleColor.Gray;


            SlowPrint("\n   ====== Elektronicky zoznam elektroinstalacneho materialu ======", 10);
            SlowPrint("    \tBakalarska praca | Verzia 1.2.2", 10);

            Console.WriteLine("\n");
            SlowPrint("   Stlačte ENTER pre pokračovanie do softwaru...", 10);

            Console.ReadLine();
            Console.Clear();
        }


        private void SlowPrint(string message, int delay = 30)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }


}