using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using admin_mode.Models;
using Microsoft.Ajax.Utilities;

namespace admin_mode.My_custom.Helpers
{
    public static class MyHelper
    {
        public static IHtmlString LabelWithMark(this HtmlHelper helper, string content)
        { 
            var label = new TagBuilder("label");
            var mark = new TagBuilder("mark");

            mark.SetInnerText(content);

            label.InnerHtml = mark.ToString();
             
            return MvcHtmlString.Create(mark.ToString());
             
        }

        public static MvcHtmlString DisplayComboItemsIndd(this AjaxHelper ajaxHelper, ICollection<ComboItem> comboItems)
        { 
            var dd = new TagBuilder("dd");
            string result = "-";
            if (comboItems.Count == 0)
                dd.SetInnerText(result);
            else
            {
                result = "";
                foreach (var comboItem in comboItems)
                {
                    dd.SetInnerText(comboItem.Name);
                    result = result + dd;
                }
                return MvcHtmlString.Create(result);
            } 
            return MvcHtmlString.Create(dd.ToString());
        }
        public static MvcHtmlString DisplayStringIndd(this AjaxHelper ajaxHelper, ICollection<string> stringCollection)
        {
            var dd = new TagBuilder("dd");
            string result = "-";
            if (stringCollection.Count == 0)
                dd.SetInnerText(result);
            else
            {
                result = "";
                foreach (var comboItem in stringCollection)
                {
                    dd.SetInnerText(comboItem);
                    result = result + dd;
                }
                return MvcHtmlString.Create(result);
            }
            return MvcHtmlString.Create(dd.ToString());
        }
        public static IHtmlString DisplayPhoneNumber(this AjaxHelper ajaxHelper,string phonenumber)
        {
            var dd = "-";   
            if (!phonenumber.IsNullOrWhiteSpace()) 
                dd = phonenumber; 

            return MvcHtmlString.Create(dd);
        }


        public static MvcHtmlString MactionLinkHtml(this AjaxHelper ajaxHelper, string linkText, string actionName,
                                                    string controllerName, object routeValues, AjaxOptions ajaxOptions, 
                                                    object htmlAttributes)
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }


        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                                       string text,
                                                       string title,
                                                       string action,
                                                       string controller,
                                                       object routeValues = null,
                                                       object htmlAttributes = null)
        {
            //example @Html.NoEncodeActionLink("<span class='glyphicon glyphicon-plus'></span>", "the title", "UserEdit", "AdminMainPage", routeValues: new { id = @Model.Id }, htmlAttributes: new { data_modal = "", @class = "btn btn-primary" }))
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder tagBuilder  = new TagBuilder("a");
            tagBuilder.InnerHtml = text;
            tagBuilder.Attributes["title"] = title;
            tagBuilder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            tagBuilder.MergeAttributes(new RouteValueDictionary(
                         HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
             
            return MvcHtmlString.Create(tagBuilder.ToString());
        }



        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper,
                                             string text, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            var li = new TagBuilder("li");
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");
            if (string.Equals(currentAction,action,StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(currentController,controller,StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }
            if (routeValues != null)
            {
                li.InnerHtml = (htmlAttributes != null)
                    ? htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            routeValues,
                                            htmlAttributes).ToHtmlString()
                    : htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            routeValues).ToHtmlString();
            }
            else
            {
                li.InnerHtml = (htmlAttributes != null)
                    ? htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            null,
                                            htmlAttributes).ToHtmlString()
                    : htmlHelper.ActionLink(text,
                                            action,
                                            controller).ToHtmlString();
            }
            return MvcHtmlString.Create(li.ToString());
        }
    }
}