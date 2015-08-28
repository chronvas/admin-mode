using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace admin_mode.My_custom.Helpers
{
    public static class MyHelper
    {
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
            if (string.Equals(currentAction,
                              action,
                              StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController,
                              controller,
                              StringComparison.OrdinalIgnoreCase))
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