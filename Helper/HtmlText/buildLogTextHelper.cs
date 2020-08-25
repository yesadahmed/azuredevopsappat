using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Helper.HtmlText
{
    public class buildLogTextHelper
    {

        public static string TaskTitle { get { return "<p style='font-size:1.3125rem;'>"; } }
        public static string ParagrphEnd { get { return "</p>"; } }
        public static string H2Start { get { return "<H2>"; } }

        public static string H2End { get { return "</H2>"; } }
        public static string HtmlTagEnd { get { return "</>"; } }
        public static string HtmlLineBreak { get { return "<br/>"; } }

        public static string GreenSpanStart { get { return "<span style='color: rgba(85,163,98,1);font-size:1.3125rem;'>"; } }
        public static string spanEnd { get { return "</span>"; } }

        public static string RedSpanStart { get { return "<span style='color: rgba(205,74,69,1);font-size:1.3125rem; '>"; } }
       
        public static string Extraspace { get { return "&nbsp;&nbsp;"; } }

        public static string FailImage { get { return "<svg aria-labelledby='__bolt-status-507-desc' class='bolt-status flex-noshrink failed icon-large-margin' height='16' role='img' viewBox='0 0 16 16' width='16' xmlns='http://www.w3.org/2000/svg'><desc id='__bolt-status-507-desc'>Failed</desc><circle cx='8' cy='8' r='8'></circle><path d='M10.984 5.004a.9.9 0 0 1 0 1.272L9.27 7.99l1.74 1.741a.9.9 0 1 1-1.272 1.273l-1.74-1.741-1.742 1.74a.9.9 0 1 1-1.272-1.272l1.74-1.74-1.713-1.714a.9.9 0 0 1 1.273-1.273l1.713 1.713 1.714-1.713a.9.9 0 0 1 1.273 0z' fill='#fff'></path></svg>"; } }
        public static string SuccessImage { get { return "<svg aria-labelledby='__bolt-status-511-desc' class='bolt-status flex-noshrink success icon-large-margin' height='16' role='img' viewBox='0 0 16 16' width='16' xmlns='http://www.w3.org/2000/svg'><desc id='__bolt-status-511-desc'>Success</desc><circle cx='8' cy='8' r='8'></circle><path d='M6.062 11.144l-.003-.002-1.784-1.785A.937.937 0 1 1 5.6 8.031l1.125 1.124 3.88-3.88A.937.937 0 1 1 11.931 6.6l-4.54 4.54-.004.004a.938.938 0 0 1-1.325 0z' fill='#fff'></path></svg>"; } }

             public static    string InsertHtmlStartform()
        {
            return @"<!DOCTYPE html>
<html>
<head>
<style>
.text-ellipsis {
   
    white-space: nowrap;
}
.bolt-status {
    fill: currentColor;
}
.bolt-status.failed {
    color: rgba(205,74,69,1);
    color: var(--component-status-error,rgba(205, 74, 69, 1));
}.bolt-status.success {
    color: rgba(85,163,98,1);
    color: var(--component-status-success,rgba(85, 163, 98, 1));
}
</style>
</head><body style='background-color:black; color:white;font-family: Segoe UI VSS (Regular),Segoe UI,-apple-system,BlinkMacSystemFont,Roboto,Helvetica Neue,Helvetica,Ubuntu,Arial,sans-serif,Apple Color Emoji,Segoe UI Emoji,Segoe UI Symbol;'>";
        }




      public static  string InsertHtmlEndForm()
        {
            return "</body></html>";
        }
    }
}
