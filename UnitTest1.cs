using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Search;

    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ExampleTest : PageTest
{
     [SetUp]
        public async Task Setup()
        {
            await Page.GotoAsync("https://web-dev.bluebnc.com/en-us");
        }


    [Test]
    public async  Task SearchBar()
    {
         await Page.WaitForSelectorAsync("input#SearchDestination.form-input");

            
            await Page.ClickAsync("input#SearchDestination.form-input");
            await Task.Delay(2000);

              await Page.WaitForSelectorAsync("ul.list.open");

    
            var listItems = await Page.QuerySelectorAllAsync("ul.list.open > li.list-item");

       
            if (listItems.Any())
            {
                //selecting a  random item from list
              var random = new Random();
                var randomIndex = random.Next(listItems.Count);
                var randomItem = listItems[randomIndex];

                //extracting the text of the selected item
                var itemTitleElement = await randomItem.QuerySelectorAsync("span.item-title");
                var itemSubTitleElement = await randomItem.QuerySelectorAsync("span.item-subTitle");
                var itemTitle = await itemTitleElement.InnerTextAsync();
                var itemSubTitle = await itemSubTitleElement.InnerTextAsync();
                var expectedValue = $"{itemTitle}, {itemSubTitle}";
                     await randomItem.ClickAsync();

       
                await Task.Delay(3000);

            
                await Page.WaitForSelectorAsync("input#destination-search.form-input");

            
                var inputValue = await Page.GetAttributeAsync("input#destination-search.form-input", "value");

                //verifying that the input value matches the selected list item value
            

                Assert.That(expectedValue, Is.EqualTo(inputValue));
            }
            else
            {
                Assert.Fail("No list items found.");
            }
    
    }


       [TearDown]
        public async Task TearDown()
        {
           
            await Page.CloseAsync();
        }
}