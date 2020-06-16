using MC1000.Data;
using MC1000.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MC1000.Controllers
{
    public class CRON : ICRON
    {
        private readonly ApplicationDbContext _context;

        public CRON(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DailyCRON()
        {
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("en-GB");

            //Load products into DB
            XDocument xdocProduct = XDocument.Load("http://supermaco.starwave.nl/api/products");

            var products = xdocProduct.Descendants("Product");
            foreach (var product in products)
            {
                var productEAN = product.Descendants("EAN").First().Value;
                if (!_context.Product.Any(u => u.EAN == productEAN))
                {
                    Product p = new Product
                    {
                        EAN = product.Descendants("EAN").First().Value,
                        Title = product.Descendants("Title").First().Value,
                        Brand = product.Descendants("Brand").First().Value,
                        ShortDescription = product.Descendants("Shortdescription").First().Value,
                        FullDescription = product.Descendants("Fulldescription").First().Value,
                        Image = product.Descendants("Image").First().Value,
                        Weight = product.Descendants("Weight").First().Value,
                        Price = double.Parse(product.Descendants("Price").First().Value, style, provider),
                        SubSub = product.Descendants("Subsubcategory").First().Value
                    };
                    _context.Add(p);
                }
            }
            //=========================

            //Load Categories into DB
            XDocument xdocCategory = XDocument.Load("http://supermaco.starwave.nl/api/categories");

            var categories = xdocCategory.Descendants("Category");
            foreach (var category in categories)
            {
                var categoryName = category.Descendants("Name").First().Value;

                if (!_context.Category.Any(u => u.Name == categoryName)) //check for dupe
                {
                    Category c = new Category
                    {
                        Name = category.Descendants("Name").First().Value
                    };

                    //Load Subcategories to DB and link to Category
                    var subCategories = category.Descendants("Subcategory");

                    c.SubCategories = new List<SubCategory>();
                    foreach (var sub in subCategories)
                    {
                        var subcategoryName = sub.Descendants("Name").First().Value;
                        SubCategory s = new SubCategory();
                        if (!_context.SubCategory.Any(u => u.Name == subcategoryName)) //check for dupe
                        {
                            s.Name = sub.Descendants("Name").First().Value;
                            s.Category = c;
                            c.SubCategories.Add(s);
                        }

                        //Load SubSubcategories to DB and link to SubCategory
                        var subSubCategories = sub.Descendants("Subsubcategory");

                        s.SubSubCategories = new List<SubSubCategory>();

                        foreach (var subsub in subSubCategories)
                        {
                            //elke subsub.name zit momenteel in elke sub
                            var subsubcategoryName = subsub.Descendants("Name").First().Value;
                            SubSubCategory u = new SubSubCategory();
                            if (!_context.SubSubCategory.Any(u => u.Name == subsubcategoryName)) //check for dupe
                            {
                                u.Name = subsubcategoryName;
                                u.SubCategory = s;
                                s.SubSubCategories.Add(u);
                            }
                        }
                    }
                    _context.Add(c);
                }
            }
            //Load promotions to DB
            XDocument xdocPromo = XDocument.Load("http://supermaco.starwave.nl/api/promotions");
            var promotions = xdocPromo.Descendants("Promotion");
            foreach (var promotion in promotions)
            {
                Promotion p = new Promotion
                {
                    Title = promotion.Descendants("Title").First().Value
                };
                if (!_context.Promotion.Any(u => u.Title == p.Title))
                {
                    //Load discounts to DB and link to Promotion
                    var discounts = promotion.Descendants("Discount");
                    p.Discounts = new List<Discount>();
                    foreach (var discount in discounts)
                    {
                        var discountedPrice = Decimal.Parse(discount.Descendants("DiscountPrice").First().Value, style, provider);
                        var discountValid = DateTime.Parse(discount.Descendants("ValidUntil").First().Value);
                        var EAN = discount.Descendants("EAN").First().Value;

                        if (!_context.Discount.Any(u => u.DiscountedPrice == discountedPrice &&
                        u.ValidUntil == discountValid && u.EAN == EAN))
                        {
                            Discount d = new Discount
                            {
                                EAN = discount.Descendants("EAN").First().Value,
                                //var proId = _context.Product.Where(z => z.EAN == d.EAN).FirstOrDefault().Id;

                                //d.ProductId = proId;

                                DiscountedPrice = Decimal.Parse(discount.Descendants("DiscountPrice").First().Value, style, provider),
                                ValidUntil = DateTime.Parse(discount.Descendants("ValidUntil").First().Value)
                            };
                            p.Discounts.Add(d);
                        }
                    }
                    _context.Add(p);
                }
            }

            //Load deliveryslots into DB
            XDocument xdocDeliveryslot = XDocument.Load("http://supermaco.starwave.nl/api/deliveryslots");

            var deliveryslots = xdocDeliveryslot.Descendants("Deliveryslot");
            foreach (var deliveryslot in deliveryslots)
            {
                var date = DateTime.Parse(deliveryslot.Descendants("Date").First().Value);

                if (!_context.DeliverySlot.Any(u => u.DeliveryDate == date)) // check for dupe
                {
                    DeliverySlot d = new DeliverySlot();
                    {
                        d.DeliveryDate = DateTime.Parse(deliveryslot.Descendants("Date").First().Value);

                        var timeslots = deliveryslot.Descendants("Timeslot");

                        d.TimeSlots = new List<TimeSlot>();
                        foreach (var timeslot in timeslots)
                        {
                            var startTime = DateTime.Parse(timeslot.Descendants("StartTime").First().Value);
                            var endTime = DateTime.Parse(timeslot.Descendants("EndTime").First().Value);
                            var price = Decimal.Parse(timeslot.Descendants("Price").First().Value, style, provider);

                            if (!_context.TimeSlot.Any(t => t.Price == price && t.StartTime == startTime && t.EndTime == endTime)) // check for dupe
                            {
                                TimeSlot t = new TimeSlot
                                {
                                    StartTime = DateTime.Parse(timeslot.Descendants("StartTime").First().Value),
                                    EndTime = DateTime.Parse(timeslot.Descendants("EndTime").First().Value),
                                    Price = Decimal.Parse(timeslot.Descendants("Price").First().Value, style, provider)
                                };
                                d.TimeSlots.Add(t);
                            }
                        }
                    }
                    _context.Add(d);
                }
            }
            //Deliveryslots Loaded

            _context.SaveChanges();
            Console.WriteLine($"Hangfire heeft succesvol de XML data geupdate");
        }
    }
}