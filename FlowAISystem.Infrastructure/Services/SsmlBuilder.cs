using System;
using System.Xml.Linq;

namespace FlowAISystem.Infrastructure.Services;

public static class SsmlBuilder
{
    public static string BuildSsml(
        string text,
        string voiceName = "en-US-JennyNeural",
        string rate = "+0.00%",
        string pitch = "+0.00Hz",
        string? style = null,
        double? styleDegree = null,
        string locale = "en-US")
    {
        XNamespace ms = "http://www.w3.org/2001/10/synthesis";
        XNamespace mstts = "http://www.w3.org/2001/mstts";

        var prosodyElement = new XElement(ms + "prosody",
            new XAttribute("rate", rate),
            new XAttribute("pitch", pitch),
            text
        );

        XElement innerContent;
        if (!string.IsNullOrWhiteSpace(style) && !style.Equals("general", StringComparison.OrdinalIgnoreCase))
        {
            var expressAs = new XElement(mstts + "express-as",
                new XAttribute("style", style)
            );

            if (styleDegree.HasValue)
            {
                double clampedDegree = Math.Clamp(styleDegree.Value, 0.01, 2.0);
                expressAs.Add(new XAttribute("styledegree", clampedDegree.ToString("F2")));
            }

            expressAs.Add(prosodyElement);
            innerContent = expressAs;
        }
        else
        {
            innerContent = prosodyElement;
        }

        var ssmlDoc = new XDocument(
            new XElement(ms + "speak",
                new XAttribute("version", "1.0"),
                new XAttribute(XNamespace.Xmlns + "mstts", mstts.NamespaceName),
                new XAttribute("xml:lang", locale),
                new XElement(ms + "voice",
                    new XAttribute("name", voiceName),
                    innerContent
                )
            )
        );

        return ssmlDoc.ToString(SaveOptions.DisableFormatting);
    }
}