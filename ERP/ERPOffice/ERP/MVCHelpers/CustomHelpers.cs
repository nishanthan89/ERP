using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MvcHelpers
{
    public static class CustomHelpers
    {
        public static MvcHtmlString DialogFormLink(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
                string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLink");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString DialogFormLinkList(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
            string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkList");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString DialogFormLinkEdit(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
             string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkEdit");

            return new MvcHtmlString(builder.ToString());
        }


        public static MvcHtmlString DialogFormLinkDelete(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
            string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkDelete");

            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString DialogFormLinkClose(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
            string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkClose");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString DialogFormLinkCheck(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
            string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkAdd");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString DialogFormLinkApproved(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
         string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkApprove");

            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString DialogFormLinkNotepad(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
            string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinknotepad");

            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString DialogFormLinkUpdate(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
         string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkUpdate");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString DialogFormLinkDeleteOption(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
              string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinkDeleteOption");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString DialogFormLinkNotepadPDA(this HtmlHelper htmlHelper, string linkText, string dialogContentUrl,
         string dialogTitle, string height, string width, string updateTargetId, string updateUrl)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(linkText);
            builder.Attributes.Add("href", dialogContentUrl);
            builder.Attributes.Add("data-dialog-title", dialogTitle);
            builder.Attributes.Add("data-update-target-id", updateTargetId);
            builder.Attributes.Add("data-update-url", updateUrl);
            builder.Attributes.Add("DialogHeight", height);
            builder.Attributes.Add("DialogWidth", width);

            // Add a css class named dialogLink that will be
            // used to identify the anchor tag and to wire up
            // the jQuery functions
            builder.AddCssClass("dialogLinknotepadPDA");

            return new MvcHtmlString(builder.ToString());
        }
    }
}