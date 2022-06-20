// See https://aka.ms/new-console-template for more information

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

var options = new ChromeOptions();
var service = ChromeDriverService.CreateDefaultService();
service.HideCommandPromptWindow = true;
options.AddArgument("no-sandbox");
options.AddArgument("remote-debugging-port=0");
options.AddArgument("disable-extensions");

var chrome = new ChromeDriver(service, options);

chrome.Navigate().GoToUrl("https://account.mail.ru/login");
Console.Read();
chrome.Navigate().GoToUrl("https://account.mail.ru/user/2-step-auth/passwords");
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[1]/div/div[1]/div[5]/div/a"))).Click();
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[1]/div/div[1]/div[4]/form/div[1]/div/div[1]/div[1]/input"))).SendKeys(RandomString(5));

(await WaitAvaibleElement(chrome, By.XPath("//*[@id='account-content']/div[3]/div[1]/div/div[1]/div[4]/form/div[4]/div[1]/button"))).Click();
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[1]/div/div[1]/div[4]/form/div[2]/div/div[2]/div[1]/input"))).SendKeys("zj324hnxbmekfl3flw3liibgp73uo4");


//Console.WriteLine((await WaitAvaibleElement(chrome, By.XPath("//*[@id='account-content']/div[3]/div[1]/div/div[1]/div[4]/div"))).GetAttribute("textContent"));
return;

chrome.Navigate().GoToUrl("https://account.mail.ru/signup");

(await WaitAvaibleElement(chrome, By.XPath("//*[@id='fname']"))).SendKeys(RandomString(10));//First name
(await WaitAvaibleElement(chrome, By.XPath("//*[@id='lname']"))).SendKeys(RandomString(10));//Last name

//Select day
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[3]/div[4]/div/div/div/div/form/div[5]/div[2]/div/div[1]/div/div/div"))).Click();
var elementDay = (await WaitAvaibleElement(chrome, By.CssSelector("[data-test-id=select-menu-wrapper]")));
elementDay.FindElement(By.TagName("DIV")).FindElement(By.TagName("DIV"))
    .FindElement(By.TagName("DIV")).FindElement(By.TagName("DIV")).FindElement(By.Id($"react-select-2-option-{new Random().Next(0, 30)}")).Click();
//Select month 
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[3]/div[4]/div/div/div/div/form/div[5]/div[2]/div/div[3]/div/div/div"))).Click();
var elementMonth = (await WaitAvaibleElement(chrome, By.CssSelector("[data-test-id=select-menu-wrapper]")));
elementMonth.FindElement(By.TagName("DIV")).FindElement(By.TagName("DIV"))
    .FindElement(By.TagName("DIV")).FindElement(By.TagName("DIV")).FindElement(By.Id($"react-select-3-option-{new Random().Next(0, 11)}")).Click();
//Select year 
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[3]/div[4]/div/div/div/div/form/div[5]/div[2]/div/div[5]/div/div/div/div"))).Click();
var elementYear = (await WaitAvaibleElement(chrome, By.CssSelector("[data-test-id=select-menu-wrapper]")));
elementYear.FindElement(By.TagName("DIV")).FindElement(By.TagName("DIV"))
    .FindElement(By.TagName("DIV")).FindElement(By.TagName("DIV")).FindElement(By.Id($"react-select-4-option-{new Random().Next(22, 121)}")).Click();//From 2000 - 1901

(await WaitAvaibleElement(chrome, By.XPath($"/html/body/div[1]/div[3]/div[3]/div[4]/div/div/div/div/form/div[8]/div[2]/div/label[{new Random().Next(1, 2)}]"))).Click();//Choose gender

var login = RandomString(20);

(await WaitAvaibleElement(chrome, By.XPath("//*[@id='aaa__input']"))).SendKeys(login);//Enter email

var pass = RandomString(30);

(await WaitAvaibleElement(chrome, By.XPath("//*[@id='password']"))).SendKeys(pass);//Enter password
(await WaitAvaibleElement(chrome, By.XPath("//*[@id='repeatPassword']"))).SendKeys(pass);//Enter confirm password
(await WaitAvaibleElement(chrome, By.XPath("//*[@id='phone-number__phone-input']"))).SendKeys("9773191529");//Enter phone number

(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[3]/div[4]/div/div/div/div/form/button"))).Click();//Submit

chrome.SwitchTo().Frame(await WaitAvaibleElement(chrome,
    By.XPath("/html/body/div[1]/div[3]/div[3]/div[3]/div/div/div/form/div[3]/div/div/div/iframe")));
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[2]/div[3]/div[1]/div/div/span/div[1]"))).Click();//Resolve captcha

(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[3]/div[3]/div/div/div/form/div[5]/div/div/div/div/input"))).SendKeys("8800");//Enter code sms
(await WaitAvaibleElement(chrome, By.XPath("/html/body/div[1]/div[3]/div[3]/div[3]/div/div/div/form/button[1]"))).Click();//Submit

Console.WriteLine($"{login}:{pass}");

string RandomString(int length)
{
    const string chars = "qwertyuiopasdfghjklzxcvbnm0123456789";
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[new Random().Next(s.Length)]).ToArray());
}

async Task<IWebElement> WaitAvaibleElement(ISearchContext driver, By by)
{
    while (true)
    {
        await Task.Delay(500);
        var info = IsElementExist(driver, by);
        
        if (!info.isAvaible)
            continue;
        
        return info.element;
    }
}

(bool isAvaible, IWebElement element) IsElementExist(ISearchContext driver, By by)
{
    try
    {
        var element = driver.FindElement(by);
        return (element.Displayed, element);
    }
    catch (NoSuchElementException)
    {
        return (false, null)!;
    }
}