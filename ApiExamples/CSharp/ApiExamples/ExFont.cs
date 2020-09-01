﻿// Copyright (c) 2001-2020 Aspose Pty Ltd. All Rights Reserved.
//
// This file is part of Aspose.Words. The source code in this file
// is only intended as a supplement to the documentation, and is provided
// "as is", without warranty of any kind, either expressed or implied.
//////////////////////////////////////////////////////////////////////////

#if NET462 || NETCOREAPP2_1 || JAVA
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Fields;
using Aspose.Words.Fonts;
using Aspose.Words.Tables;
using NUnit.Framework;

namespace ApiExamples
{
    [TestFixture]
    public class ExFont : ApiExampleBase
    {
        [Test]
        public void CreateFormattedRun()
        {
            //ExStart
            //ExFor:Document.#ctor
            //ExFor:Font
            //ExFor:Font.Name
            //ExFor:Font.Size
            //ExFor:Font.HighlightColor
            //ExFor:Run
            //ExFor:Run.#ctor(DocumentBase,String)
            //ExFor:Story.FirstParagraph
            //ExSummary:Shows how to format a run of text using its font property.
            Document doc = new Document();
            Run run = new Run(doc, "Hello world!");

            Aspose.Words.Font font = run.Font;
            font.Name = "Courier New";
            font.Size = 36;
            font.HighlightColor = Color.Yellow;

            doc.FirstSection.Body.FirstParagraph.AppendChild(run);
            doc.Save(ArtifactsDir + "Font.CreateFormattedRun.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.CreateFormattedRun.docx");
            run = doc.FirstSection.Body.FirstParagraph.Runs[0];

            Assert.AreEqual("Hello world!", run.GetText().Trim());
            Assert.AreEqual("Courier New", run.Font.Name);
            Assert.AreEqual(36, run.Font.Size);
            Assert.AreEqual(Color.Yellow.ToArgb(), run.Font.HighlightColor.ToArgb());

        }

        [Test]
        public void Caps()
        {
            //ExStart
            //ExFor:Font.AllCaps
            //ExFor:Font.SmallCaps
            //ExSummary:Shows how to format a run to display its contents in capitals.
            Document doc = new Document();
            Paragraph para = (Paragraph)doc.GetChild(NodeType.Paragraph, 0, true);

            // There are two ways of getting a run to display its lowercase text in uppercase without changing the contents.
            // 1 -  Set the AllCaps flag to display all characters in regular capitals:
            Run run = new Run(doc, "all capitals");
            run.Font.AllCaps = true;
            para.AppendChild(run);

            para = (Paragraph)para.ParentNode.AppendChild(new Paragraph(doc));

            // 2 -  Set the SmallCaps flag to display all characters in small capitals:
            // If a character is lower case, it will appear in its upper case form,
            // but will have the same height it had when it was lower case (the font's x-height).
            // Characters that were in upper case originally will look the same.
            run = new Run(doc, "Small Capitals");
            run.Font.SmallCaps = true;
            para.AppendChild(run);

            doc.Save(ArtifactsDir + "Font.Caps.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Caps.docx");
            run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("all capitals", run.GetText().Trim());
            Assert.True(run.Font.AllCaps);

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("Small Capitals", run.GetText().Trim());
            Assert.True(run.Font.SmallCaps);
        }

        [Test]
        public void GetDocumentFonts()
        {
            //ExStart
            //ExFor:FontInfoCollection
            //ExFor:DocumentBase.FontInfos
            //ExFor:FontInfo
            //ExFor:FontInfo.Name
            //ExFor:FontInfo.IsTrueType
            //ExSummary:Shows how to print the details of what fonts are present in a document.
            Document doc = new Document(MyDir + "Embedded font.docx");

            FontInfoCollection allFonts = doc.FontInfos;
            Assert.AreEqual(5, allFonts.Count); //ExSkip

            // Print all the used and unused fonts in the document.
            for (int i = 0; i < allFonts.Count; i++)
            {
                Console.WriteLine($"Font index #{i}");
                Console.WriteLine($"\tName: {allFonts[i].Name}");
                Console.WriteLine($"\tIs {(allFonts[i].IsTrueType ? "" : "not ")}a trueType font");
            }
            //ExEnd
        }

        [Test]
        [Description("WORDSNET-16234")]
        public void DefaultValuesEmbeddedFontsParameters()
        {
            Document doc = new Document();

            Assert.IsFalse(doc.FontInfos.EmbedTrueTypeFonts);
            Assert.IsFalse(doc.FontInfos.EmbedSystemFonts);
            Assert.IsFalse(doc.FontInfos.SaveSubsetFonts);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void FontInfoCollection(bool embedAllFonts)
        {
            //ExStart
            //ExFor:FontInfoCollection
            //ExFor:DocumentBase.FontInfos
            //ExFor:FontInfoCollection.EmbedTrueTypeFonts
            //ExFor:FontInfoCollection.EmbedSystemFonts
            //ExFor:FontInfoCollection.SaveSubsetFonts
            //ExSummary:Shows how to save a document with embedded TrueType fonts.
            Document doc = new Document(MyDir + "Document.docx");

            FontInfoCollection fontInfos = doc.FontInfos;
            fontInfos.EmbedTrueTypeFonts = embedAllFonts;
            fontInfos.EmbedSystemFonts = embedAllFonts;
            fontInfos.SaveSubsetFonts = embedAllFonts;

            doc.Save(ArtifactsDir + "Font.FontInfoCollection.docx");

            FileInfo fileInfo = new FileInfo(ArtifactsDir + "Font.FontInfoCollection.docx");

            if (embedAllFonts)
                Assert.True(fileInfo.Length > 23000);
            else
                Assert.True(fileInfo.Length < 9000);
            //ExEnd
        }

        [TestCase(true, false, false, Description =
            "Save a document with embedded TrueType fonts. System fonts are not included. Saves full versions of embedding fonts.")]
        [TestCase(true, true, false, Description =
            "Save a document with embedded TrueType fonts. System fonts are included. Saves full versions of embedding fonts.")]
        [TestCase(true, true, true, Description =
            "Save a document with embedded TrueType fonts. System fonts are included. Saves subset of embedding fonts.")]
        [TestCase(true, false, true, Description =
            "Save a document with embedded TrueType fonts. System fonts are not included. Saves subset of embedding fonts.")]
        [TestCase(false, false, false, Description = "Remove embedded fonts from the saved document.")]
        public void WorkWithEmbeddedFonts(bool embedTrueTypeFonts, bool embedSystemFonts, bool saveSubsetFonts)
        {
            Document doc = new Document(MyDir + "Document.docx");

            FontInfoCollection fontInfos = doc.FontInfos;
            fontInfos.EmbedTrueTypeFonts = embedTrueTypeFonts;
            fontInfos.EmbedSystemFonts = embedSystemFonts;
            fontInfos.SaveSubsetFonts = saveSubsetFonts;

            doc.Save(ArtifactsDir + "Font.WorkWithEmbeddedFonts.docx");
        }

        [Test]
        public void StrikeThrough()
        {
            //ExStart
            //ExFor:Font.StrikeThrough
            //ExFor:Font.DoubleStrikeThrough
            //ExSummary:Shows how to add a line strikethrough to text.
            Document doc = new Document();
            Paragraph para = (Paragraph)doc.GetChild(NodeType.Paragraph, 0, true);

            Run run = new Run(doc, "Text with a single-line strikethrough.");
            run.Font.StrikeThrough = true;
            para.AppendChild(run);

            para = (Paragraph)para.ParentNode.AppendChild(new Paragraph(doc));

            run = new Run(doc, "Text with a double-line strikethrough.");
            run.Font.DoubleStrikeThrough = true;
            para.AppendChild(run);

            doc.Save(ArtifactsDir + "Font.StrikeThrough.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.StrikeThrough.docx");

            run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Text with a single-line strikethrough.", run.GetText().Trim());
            Assert.True(run.Font.StrikeThrough);

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("Text with a double-line strikethrough.", run.GetText().Trim());
            Assert.True(run.Font.DoubleStrikeThrough);
        }

        [Test]
        public void PositionSubscript()
        {
            //ExStart
            //ExFor:Font.Position
            //ExFor:Font.Subscript
            //ExFor:Font.Superscript
            //ExSummary:Shows how to format text to offset its position.
            Document doc = new Document();
            Paragraph para = (Paragraph) doc.GetChild(NodeType.Paragraph, 0, true);

            // Add a run of text that is raised 5 points above the baseline.
            Run run = new Run(doc, "Raised text. ");
            run.Font.Position = 5;
            para.AppendChild(run);

            // Add a run of text that is lowered 10 points below the baseline.
            run = new Run(doc, "Lowered text. ");
            run.Font.Position = -10;
            para.AppendChild(run);

            // Add a run of normal text.
            run = new Run(doc, "Text in its default position. ");
            para.AppendChild(run);

            // Add a run of text that appears as subscript.
            run = new Run(doc, "Subscript. ");
            run.Font.Subscript = true;
            para.AppendChild(run);

            // Add a run of text that appears as superscript.
            run = new Run(doc, "Superscript.");
            run.Font.Superscript = true;
            para.AppendChild(run);

            doc.Save(ArtifactsDir + "Font.PositionSubscript.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.PositionSubscript.docx");
            run = doc.FirstSection.Body.FirstParagraph.Runs[0];

            Assert.AreEqual("Raised text.", run.GetText().Trim());
            Assert.AreEqual(5, run.Font.Position);

            doc = new Document(ArtifactsDir + "Font.PositionSubscript.docx");
            run = doc.FirstSection.Body.FirstParagraph.Runs[1];

            Assert.AreEqual("Lowered text.", run.GetText().Trim());
            Assert.AreEqual(-10, run.Font.Position);

            run = doc.FirstSection.Body.FirstParagraph.Runs[3];

            Assert.AreEqual("Subscript.", run.GetText().Trim());
            Assert.True(run.Font.Subscript);

            run = doc.FirstSection.Body.FirstParagraph.Runs[4];

            Assert.AreEqual("Superscript.", run.GetText().Trim());
            Assert.True(run.Font.Superscript);
        }

        [Test]
        public void ScalingSpacing()
        {
            //ExStart
            //ExFor:Font.Scaling
            //ExFor:Font.Spacing
            //ExSummary:Shows how to set horizontal scaling and spacing for characters.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Add run of text, and increase character width to 150%.
            builder.Font.Scaling = 150;
            builder.Writeln("Wide characters");

            // Add run of text, and add 1pt of extra horizontal spacing between each character.
            builder.Font.Spacing = 1;
            builder.Writeln("Expanded by 1pt");

            // Add run of text, and bring characters closer together by 1pt.
            builder.Font.Spacing = -1;
            builder.Writeln("Condensed by 1pt");

            doc.Save(ArtifactsDir + "Font.ScalingSpacing.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.ScalingSpacing.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Wide characters", run.GetText().Trim());
            Assert.AreEqual(150, run.Font.Scaling);

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("Expanded by 1pt", run.GetText().Trim());
            Assert.AreEqual(1, run.Font.Spacing);

            run = doc.FirstSection.Body.Paragraphs[2].Runs[0];

            Assert.AreEqual("Condensed by 1pt", run.GetText().Trim());
            Assert.AreEqual(-1, run.Font.Spacing);
        }

        [Test]
        public void Italic()
        {
            //ExStart
            //ExFor:Font.Italic
            //ExSummary:Shows how to write italicized text using a document builder.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Size = 36;
            builder.Font.Italic = true;
            builder.Writeln("Hello world!");

            doc.Save(ArtifactsDir + "Font.Italic.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Italic.docx");
            Run run = doc.FirstSection.Body.FirstParagraph.Runs[0];

            Assert.AreEqual("Hello world!", run.GetText().Trim());
            Assert.True(run.Font.Italic);
        }

        [Test]
        public void EngraveEmboss()
        {
            //ExStart
            //ExFor:Font.Emboss
            //ExFor:Font.Engrave
            //ExSummary:Shows how to apply engraving/embossing effects to text.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Size = 36;
            builder.Font.Color = Color.LightBlue;

            // Below are two ways of using shadows to apply a 3D-like effect to text.
            // 1 -  Engrave text to make it look like the letters are sunken into the page:
            builder.Font.Engrave = true;

            builder.Writeln("This text is engraved.");

            // 2 -  Emboss text to make it look like the letters pop out of the page:
            builder.Font.Engrave = false;
            builder.Font.Emboss = true;

            builder.Writeln("This text is embossed.");

            doc.Save(ArtifactsDir + "Font.EngraveEmboss.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.EngraveEmboss.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("This text is engraved.", run.GetText().Trim());
            Assert.True(run.Font.Engrave);
            Assert.False(run.Font.Emboss);

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("This text is embossed.", run.GetText().Trim());
            Assert.False(run.Font.Engrave);
            Assert.True(run.Font.Emboss);
        }

        [Test]
        public void Shadow()
        {
            //ExStart
            //ExFor:Font.Shadow
            //ExSummary:Shows how to create a run of text formatted with a shadow.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Set the Shadow flag to apply an offset shadow effect
            // which makes it look like the letters are floating above the page.
            builder.Font.Shadow = true;
            builder.Font.Size = 36;

            builder.Writeln("This text has a shadow.");

            doc.Save(ArtifactsDir + "Font.Shadow.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Shadow.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("This text has a shadow.", run.GetText().Trim());
            Assert.True(run.Font.Shadow);
        }

        [Test]
        public void Outline()
        {
            //ExStart
            //ExFor:Font.Outline
            //ExSummary:Shows how to create a run of text formatted as outline.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Set the Outline flag to change the fill color of the text to white,
            // and to leave a thin outline around each character in the original color of the text. 
            builder.Font.Outline = true;
            builder.Font.Color = Color.Blue;
            builder.Font.Size = 36;

            builder.Writeln("This text has an outline.");

            doc.Save(ArtifactsDir + "Font.Outline.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Outline.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("This text has an outline.", run.GetText().Trim());
            Assert.True(run.Font.Outline);
        }

        [Test]
        public void Hidden()
        {
            //ExStart
            //ExFor:Font.Hidden
            //ExSummary:Shows how to create a run of hidden text.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // With the Hidden flag set to true, any text that we create using this Font object will be invisible in the document.
            // We will not be able to see or highlight hidden text unless we enable the "Hidden text" option
            // found in Microsoft Word via File -> Options -> Display. The text will still be there,
            // and we will be able to access this text programmatically.
            // It is not advised to use this method to hide sensitive information.
            builder.Font.Hidden = true;
            builder.Font.Size = 36;
            
            builder.Writeln("This text will not be visible in the document.");

            doc.Save(ArtifactsDir + "Font.Hidden.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Hidden.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("This text will not be visible in the document.", run.GetText().Trim());
            Assert.True(run.Font.Hidden);
        }

        [Test]
        public void Kerning()
        {
            //ExStart
            //ExFor:Font.Kerning
            //ExSummary:Shows how to specify the font size at which kerning begins to take effect.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.Font.Name = "Arial Black";

            // Set the builder's font size, and minimum size at which kerning will be applied.
            // The font size falls below the kerning threshold, so kerning will not be applied.
            builder.Font.Size = 18;
            builder.Font.Kerning = 24;

            builder.Writeln("TALLY. (Kerning not applied)");

            // Set the kerning threshold so that the builder's current font size is above it.
            // Any text we add from this point will have kerning applied. The spaces between characters
            // will be adjusted, normally resulting in a slightly more aesthetically pleasing run of text.
            builder.Font.Kerning = 12;
            
            builder.Writeln("TALLY. (Kerning applied)");

            doc.Save(ArtifactsDir + "Font.Kerning.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Kerning.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("TALLY. (Kerning not applied)", run.GetText().Trim());
            Assert.AreEqual(24, run.Font.Kerning);
            Assert.AreEqual(18, run.Font.Size);

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("TALLY. (Kerning applied)", run.GetText().Trim());
            Assert.AreEqual(12, run.Font.Kerning);
            Assert.AreEqual(18, run.Font.Size);
        }

        [Test]
        public void NoProofing()
        {
            //ExStart
            //ExFor:Font.NoProofing
            //ExSummary:Shows how to prevent text from being spell checked by Microsoft Word.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Normally, Microsoft Word emphasizes spelling errors with a red jagged underline.
            // We can un-set the NoProofing flag in order to create a portion of text
            // which bypasses the spell checker while avoiding disabling it completely.
            builder.Font.NoProofing = true;

            builder.Writeln("Proofing has been disabled, so these spelking errrs will not display red lines underneath.");

            doc.Save(ArtifactsDir + "Font.NoProofing.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.NoProofing.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Proofing has been disabled, so these spelking errrs will not display red lines underneath.", run.GetText().Trim());
            Assert.True(run.Font.NoProofing);
        }

        [Test]
        public void LocaleId()
        {
            //ExStart
            //ExFor:Font.LocaleId
            //ExSummary:Shows how to set the locale of the text that we are adding with a document builder.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // If we set the font's locale to English, and insert some text in Russian,
            // the English locale spell checker will not be able to recognize the text and detect it as a spelling error.
            builder.Font.LocaleId = new CultureInfo("en-US", false).LCID;
            builder.Writeln("Привет!");
            
            // Set a matching locale for the text that we are about to add to apply the appropriate spell checker.
            builder.Font.LocaleId = new CultureInfo("ru-RU", false).LCID;
            builder.Writeln("Привет!");

            doc.Save(ArtifactsDir + "Font.LocaleId.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.LocaleId.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Привет!", run.GetText().Trim());
            Assert.AreEqual(1033, run.Font.LocaleId);

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("Привет!", run.GetText().Trim());
            Assert.AreEqual(1049, run.Font.LocaleId);
        }

        [Test]
        public void Underlines()
        {
            //ExStart
            //ExFor:Font.Underline
            //ExFor:Font.UnderlineColor
            //ExSummary:Shows how to configure the style and color of a text underline.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Underline = Underline.Dotted;
            builder.Font.UnderlineColor = Color.Red;

            builder.Writeln("Underlined text.");

            doc.Save(ArtifactsDir + "Font.Underlines.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Underlines.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Underlined text.", run.GetText().Trim());
            Assert.AreEqual(Underline.Dotted, run.Font.Underline);
            Assert.AreEqual(Color.Red.ToArgb(), run.Font.UnderlineColor.ToArgb());
        }

        [Test]
        public void ComplexScript()
        {
            //ExStart
            //ExFor:Font.ComplexScript
            //ExSummary:Shows how to add text that is always treated as complex script.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.ComplexScript = true;

            builder.Writeln("Text treated as complex script.");

            doc.Save(ArtifactsDir + "Font.ComplexScript.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.ComplexScript.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Text treated as complex script.", run.GetText().Trim());
            Assert.True(run.Font.ComplexScript);
        }

        [Test]
        public void SparklingText()
        {
            //ExStart
            //ExFor:Font.TextEffect
            //ExSummary:Shows how to apply a visual effect to a run.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Size = 36;
            builder.Font.TextEffect = TextEffect.SparkleText;

            builder.Writeln("Text with a sparkle effect.");
            
            // Font animation effects are only supported by older versions of Microsoft Word.
            doc.Save(ArtifactsDir + "Font.SparklingText.doc");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.SparklingText.doc");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Text with a sparkle effect.", run.GetText().Trim());
            Assert.AreEqual(TextEffect.SparkleText, run.Font.TextEffect);
        }

        [Test]
        public void Shading()
        {
            //ExStart
            //ExFor:Font.Shading
            //ExSummary:Shows how to apply shading to text created by a document builder.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Color = Color.White;

            // One way to make the text created using our white font color visible
            // is to apply a background shading effect.
            Shading shading = builder.Font.Shading;
            shading.Texture = TextureIndex.TextureDiagonalUp;
            shading.BackgroundPatternColor = Color.OrangeRed;
            shading.ForegroundPatternColor = Color.DarkBlue;

            builder.Writeln("White text on an orange background with a two-tone texture.");

            doc.Save(ArtifactsDir + "Font.Shading.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Shading.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("White text on an orange background with a two-tone texture.", run.GetText().Trim());
            Assert.AreEqual(Color.White.ToArgb(), run.Font.Color.ToArgb());

            Assert.AreEqual(TextureIndex.TextureDiagonalUp, run.Font.Shading.Texture);
            Assert.AreEqual(Color.OrangeRed.ToArgb(), run.Font.Shading.BackgroundPatternColor.ToArgb());
            Assert.AreEqual(Color.DarkBlue.ToArgb(), run.Font.Shading.ForegroundPatternColor.ToArgb());
        }

        [Test]
        public void Bidi()
        {
            //ExStart
            //ExFor:Font.Bidi
            //ExFor:Font.NameBi
            //ExFor:Font.SizeBi
            //ExFor:Font.ItalicBi
            //ExFor:Font.BoldBi
            //ExFor:Font.LocaleIdBi
            //ExSummary:Shows how to define separate sets of font settings for right-to-left, and right-to-left text.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            
            // Define a set of font settings for left-to-right text.
            builder.Font.Name = "Courier New";
            builder.Font.Size = 16;
            builder.Font.Italic = false;
            builder.Font.Bold = false;
            builder.Font.LocaleId = new CultureInfo("en-US", false).LCID;

            // Define another set of font settings for right-to-left text.
            builder.Font.NameBi = "Andalus";
            builder.Font.SizeBi = 24;
            builder.Font.ItalicBi = true;
            builder.Font.BoldBi = true;
            builder.Font.LocaleIdBi = new CultureInfo("ar-AR", false).LCID;

            // We can use the Bidi flag to indicate whether the text we are about to add
            // with the document builder is right-to-left. When we add text with this flag set to true,
            // it will be formatted using the right-to-left set of font settings.
            builder.Font.Bidi = true;
            builder.Write("مرحبًا");

            // Set the flag to false, and then add left-to-right text.
            // The document builder will format these using the left-to-right set of font settings.
            builder.Font.Bidi = false;
            builder.Write(" Hello world!");

            doc.Save(ArtifactsDir + "Font.Bidi.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Bidi.docx");

            foreach (Run run in doc.FirstSection.Body.Paragraphs[0].Runs)
            {
                switch (doc.FirstSection.Body.Paragraphs[0].IndexOf(run))
                {
                    case 0:
                        Assert.AreEqual("مرحبًا", run.GetText().Trim());
                        Assert.True(run.Font.Bidi);
                        break;
                    case 1:
                        Assert.AreEqual("Hello world!", run.GetText().Trim());
                        Assert.False(run.Font.Bidi);
                        break;
                }

                Assert.AreEqual(1033, run.Font.LocaleId);
                Assert.AreEqual(16, run.Font.Size);
                Assert.AreEqual("Courier New", run.Font.Name);
                Assert.False(run.Font.Italic);
                Assert.False(run.Font.Bold);
                Assert.AreEqual(1025, run.Font.LocaleIdBi);
                Assert.AreEqual(24, run.Font.SizeBi);
                Assert.AreEqual("Andalus", run.Font.NameBi);
                Assert.True(run.Font.ItalicBi);
                Assert.True(run.Font.BoldBi);
            }
        }

        [Test]
        public void FarEast()
        {
            //ExStart
            //ExFor:Font.NameFarEast
            //ExFor:Font.LocaleIdFarEast
            //ExSummary:Shows how to insert and format text in a Far East language.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Specify font settings which the document builder will apply to any text that it inserts.
            builder.Font.Name = "Courier New";
            builder.Font.LocaleId = new CultureInfo("en-US", false).LCID;

            // Name "FarEast" equivalents for our font and locale.
            // If the builder inserts Asian characters with this Font configuration, then each run that contains
            // these characters will display them using the "FarEast" font/locale instead of the default.
            // This could be useful when a western font does not have ideal representations for Asian characters.
            builder.Font.NameFarEast = "SimSun";
            builder.Font.LocaleIdFarEast = new CultureInfo("zh-CN", false).LCID;
            
            // This text will be displayed in the default font/locale.
            builder.Writeln("Hello world!");

            // Since these are Asian characters, this run will apply our "FarEast" font/locale equivalents.
            builder.Writeln("你好世界");

            doc.Save(ArtifactsDir + "Font.FarEast.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.FarEast.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Hello world!", run.GetText().Trim());
            Assert.AreEqual(1033, run.Font.LocaleId);
            Assert.AreEqual("Courier New", run.Font.Name);
            Assert.AreEqual(2052, run.Font.LocaleIdFarEast);
            Assert.AreEqual("SimSun", run.Font.NameFarEast);

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("你好世界", run.GetText().Trim());
            Assert.AreEqual(1033, run.Font.LocaleId);
            Assert.AreEqual("SimSun", run.Font.Name);
            Assert.AreEqual(2052, run.Font.LocaleIdFarEast);
            Assert.AreEqual("SimSun", run.Font.NameFarEast);
        }

        [Test]
        public void NameAscii()
        {
            //ExStart
            //ExFor:Font.NameAscii
            //ExFor:Font.NameOther
            //ExSummary:Shows how Microsoft Word can combine two different fonts in one run.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // If a run that we use the builder to insert while using this font configuration
            // contains characters within the range of ASCII characters, it will display those characters using this font.
            builder.Font.NameAscii = "Calibri";

            // With no other font specified, the builder will also apply this font to all characters that it inserts.
            Assert.AreEqual("Calibri", builder.Font.Name);

            // Specify a font to use for all characters outside of the ASCII range.
            // Ideally, this font should have a glyph for each required non-ASCII character code.
            builder.Font.NameOther = "Courier New";

            // Insert a run with one word consisting of ASCII characters, and one word with all characters outside that range.
            // Each character will be displayed using either of the fonts, depending on .
            builder.Writeln("Hello, Привет");

            doc.Save(ArtifactsDir + "Font.NameAscii.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.NameAscii.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Hello, Привет", run.GetText().Trim());
            Assert.AreEqual("Calibri", run.Font.Name);
            Assert.AreEqual("Calibri", run.Font.NameAscii);
            Assert.AreEqual("Courier New", run.Font.NameOther);
        }

        [Test]
        public void ChangeStyle()
        {
            //ExStart
            //ExFor:Font.StyleName
            //ExFor:Font.StyleIdentifier
            //ExFor:StyleIdentifier
            //ExSummary:Shows how to change the style of existing text.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Below are two ways of referencing styles.
            // 1 -  Using the style name:
            builder.Font.StyleName = "Emphasis";
            builder.Writeln("Text originally in \"Emphasis\" style");

            // 2 -  Using a built-in style identifier:
            builder.Font.StyleIdentifier = StyleIdentifier.IntenseEmphasis;
            builder.Writeln("Text originally in \"Intense Emphasis\" style");
       
            // Convert all uses of one style to another,
            // using the above methods to reference old and new styles.
            foreach (Run run in doc.GetChildNodes(NodeType.Run, true).OfType<Run>())
            {
                if (run.Font.StyleName.Equals("Emphasis"))
                    run.Font.StyleName = "Strong";

                if (run.Font.StyleIdentifier.Equals(StyleIdentifier.IntenseEmphasis))
                    run.Font.StyleIdentifier = StyleIdentifier.Strong;
            }

            doc.Save(ArtifactsDir + "Font.ChangeStyle.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.ChangeStyle.docx");
            Run docRun = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("Text originally in \"Emphasis\" style", docRun.GetText().Trim());
            Assert.AreEqual(StyleIdentifier.Strong, docRun.Font.StyleIdentifier);
            Assert.AreEqual("Strong", docRun.Font.StyleName);

            docRun = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("Text originally in \"Intense Emphasis\" style", docRun.GetText().Trim());
            Assert.AreEqual(StyleIdentifier.Strong, docRun.Font.StyleIdentifier);
            Assert.AreEqual("Strong", docRun.Font.StyleName);
        }

        [Test]
        public void BuiltIn()
        {
            //ExStart
            //ExFor:Style.BuiltIn
            //ExSummary:Shows how to differentiate custom styles from built-in styles.
            Document doc = new Document();

            // When we create a document using Microsoft Word, or programmatically using Aspose.Words, 
            // the document will come with a collection of styles, which we can apply to its text to modify its appearance.
            // We can access these built-in styles via the document's "Styles" collection.
            // These styles will all have the "BuiltIn" flag set to "true".
            Style style = doc.Styles["Emphasis"];

            Assert.True(style.BuiltIn);

            // Create a custom style, and add it to the collection.
            // Custom styles such as this will have the "BuiltIn" flag set to "false". 
            style = doc.Styles.Add(StyleType.Character, "MyStyle");
            style.Font.Color = Color.Navy;
            style.Font.Name = "Courier New";

            Assert.False(style.BuiltIn);
            //ExEnd
        }

        [Test]
        public void Style()
        {
            //ExStart
            //ExFor:Font.Style
            //ExSummary:Applies a double underline to all runs in a document that are formatted with custom character styles.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a custom style, and apply it to text created using a document builder.
            Style style = doc.Styles.Add(StyleType.Character, "MyStyle");
            style.Font.Color = Color.Red;
            style.Font.Name = "Courier New";

            builder.Font.StyleName = "MyStyle";
            builder.Write("This text is in a custom style.");
            
            // Iterate over every run, and add a double underline to every custom style.
            foreach (Run run in doc.GetChildNodes(NodeType.Run, true).OfType<Run>())
            {
                Style charStyle = run.Font.Style;

                if (!charStyle.BuiltIn)
                    run.Font.Underline = Underline.Double;
            }

            doc.Save(ArtifactsDir + "Font.Style.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.Style.docx");
            Run docRun = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("This text is in a custom style.", docRun.GetText().Trim());
            Assert.AreEqual("MyStyle", docRun.Font.StyleName);
            Assert.False(docRun.Font.Style.BuiltIn);
            Assert.AreEqual(Underline.Double, docRun.Font.Underline);
        }

        [Test]
        public void DefaultFontInstance()
        {
            //ExStart
            //ExFor:Fonts.FontSettings.DefaultInstance
            //ExSummary:Shows how to 
            // Configure the default font settings instance to use the "Courier New" font
            // as a backup substitute in the event of an unknown font being used.
            FontSettings.DefaultInstance.SubstitutionSettings.DefaultFontSubstitution.DefaultFontName = "Courier New";

            Assert.True(FontSettings.DefaultInstance.SubstitutionSettings.DefaultFontSubstitution.Enabled);

            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Name = "Non-existent font";
            builder.Write("Hello world!");
            
            // This document does not have a FontSettings configuration. When we render the document,
            // the default FontSettings instance will be invoked in order to resolve the missing font.
            // The text using the "Non-existent font" will be rendered using "Courier New".
            Assert.Null(doc.FontSettings);

            doc.Save(ArtifactsDir + "Font.DefaultFontInstance.pdf");
            //ExEnd
        }

        //ExStart
        //ExFor:IWarningCallback
        //ExFor:DocumentBase.WarningCallback
        //ExFor:Fonts.FontSettings.DefaultInstance
        //ExSummary:Shows how to use the IWarningCallback interface to monitor font substitution warnings.
        [Test] //ExSkip
        public void SubstitutionWarning()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Name = "Times New Roman";
            builder.Writeln("Hello world!");

            FontSubstitutionWarningCollector callback = new FontSubstitutionWarningCollector();
            doc.WarningCallback = callback;

            // Store the current collection of font sources, which is applied to
            // every document without its own set of font settings specified.
            FontSourceBase[] originalFontSources = FontSettings.DefaultInstance.GetFontsSources();

            // For testing purposes, we will set Aspose.Words to look for fonts only in a folder which does not exist.
            FontSettings.DefaultInstance.SetFontsFolder(string.Empty, false);

            // When rendering the document, the will be no place to find the "Times New Roman" font.
            // This will cause a font substitution warning, which our callback will detect.
            doc.Save(ArtifactsDir + "Font.SubstitutionWarning.pdf");

            FontSettings.DefaultInstance.SetFontsSources(originalFontSources);

            Assert.AreEqual(1, callback.FontSubstitutionWarnings.Count); //ExSkip
            Assert.True(callback.FontSubstitutionWarnings[0].WarningType == WarningType.FontSubstitution);
            Assert.True(callback.FontSubstitutionWarnings[0].Description
                .Equals("Font 'Times New Roman' has not been found. Using 'Fanwood' font instead. Reason: first available font."));
        }

        public class FontSubstitutionWarningCollector : IWarningCallback
        {
            /// <summary>
            /// Called every time a warning occurs during loading/saving.
            /// </summary>
            public void Warning(WarningInfo info)
            {
                if (info.WarningType == WarningType.FontSubstitution)
                    FontSubstitutionWarnings.Warning(info);
            }

            public WarningInfoCollection FontSubstitutionWarnings = new WarningInfoCollection();
        }
        //ExEnd

        [Test]
        public void GetAvailableFonts()
        {
            //ExStart
            //ExFor:Fonts.PhysicalFontInfo
            //ExFor:FontSourceBase.GetAvailableFonts
            //ExFor:PhysicalFontInfo.FontFamilyName
            //ExFor:PhysicalFontInfo.FullFontName
            //ExFor:PhysicalFontInfo.Version
            //ExFor:PhysicalFontInfo.FilePath
            //ExSummary:Shows how to list available fonts.
            // Configure Aspose.Words to source fonts from a custom folder, and then print every available font.
            FontSourceBase[] folderFontSource = { new FolderFontSource(FontsDir, true) };
            
            foreach (PhysicalFontInfo fontInfo in folderFontSource[0].GetAvailableFonts())
            {
                Console.WriteLine("FontFamilyName : {0}", fontInfo.FontFamilyName);
                Console.WriteLine("FullFontName  : {0}", fontInfo.FullFontName);
                Console.WriteLine("Version  : {0}", fontInfo.Version);
                Console.WriteLine("FilePath : {0}\n", fontInfo.FilePath);
            }
            //ExEnd

            Assert.AreEqual(folderFontSource[0].GetAvailableFonts().Count, Directory.GetFiles(FontsDir).Count(f => f.EndsWith(".ttf")));
        }

        //ExStart
        //ExFor:Fonts.FontInfoSubstitutionRule
        //ExFor:Fonts.FontSubstitutionSettings.FontInfoSubstitution
        //ExFor:IWarningCallback
        //ExFor:IWarningCallback.Warning(WarningInfo)
        //ExFor:WarningInfo
        //ExFor:WarningInfo.Description
        //ExFor:WarningInfo.WarningType
        //ExFor:WarningInfoCollection
        //ExFor:WarningInfoCollection.Warning(WarningInfo)
        //ExFor:WarningInfoCollection.GetEnumerator
        //ExFor:WarningInfoCollection.Clear
        //ExFor:WarningType
        //ExFor:DocumentBase.WarningCallback
        //ExSummary:Shows how to set the property for finding the closest match for a missing font from the available font sources.
        [Test]
        public void EnableFontSubstitution()
        {
            // Open a document which contains text formatted with a font which does not exist in any of our font sources.
            Document doc = new Document(MyDir + "Missing font.docx");

            // Assign a callback for handling font substitution warnings.
            HandleDocumentSubstitutionWarnings substitutionWarningHandler = new HandleDocumentSubstitutionWarnings();
            doc.WarningCallback = substitutionWarningHandler;

            // Set a default font name, and enable font substitution.
            FontSettings fontSettings = new FontSettings();
            fontSettings.SubstitutionSettings.DefaultFontSubstitution.DefaultFontName = "Arial"; ;
            fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = true;

            // We will get a font substitution warning if we save a document with a missing font.
            doc.FontSettings = fontSettings;
            doc.Save(ArtifactsDir + "Font.EnableFontSubstitution.pdf");

            using (IEnumerator<WarningInfo> warnings = substitutionWarningHandler.FontWarnings.GetEnumerator()) 
                while (warnings.MoveNext()) 
                    Console.WriteLine(warnings.Current.Description);

            // We can also verify warnings in the collection, and clear them.
            Assert.AreEqual(WarningSource.Layout, substitutionWarningHandler.FontWarnings[0].Source);
            Assert.AreEqual("Font '28 Days Later' has not been found. Using 'Calibri' font instead. Reason: alternative name from document.", 
                substitutionWarningHandler.FontWarnings[0].Description);

            substitutionWarningHandler.FontWarnings.Clear();

            Assert.That(substitutionWarningHandler.FontWarnings, Is.Empty);
        }

        public class HandleDocumentSubstitutionWarnings : IWarningCallback
        {
            /// <summary>
            /// Called every time a warning occurs during loading/saving.
            /// </summary>
            public void Warning(WarningInfo info)
            {
                if (info.WarningType == WarningType.FontSubstitution)
                    FontWarnings.Warning(info);
            }

            public WarningInfoCollection FontWarnings = new WarningInfoCollection();
        }
        //ExEnd

        [Test]
        public void DisableFontSubstitution()
        {
            Document doc = new Document(MyDir + "Missing font.docx");

            // Create a new class implementing IWarningCallback and assign it to the PdfSaveOptions class
            HandleDocumentSubstitutionWarnings callback = new HandleDocumentSubstitutionWarnings();
            doc.WarningCallback = callback;

            FontSettings fontSettings = new FontSettings();
            fontSettings.SubstitutionSettings.DefaultFontSubstitution.DefaultFontName = "Arial";
            fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;

            doc.FontSettings = fontSettings;
            doc.Save(ArtifactsDir + "Font.DisableFontSubstitution.pdf");

            Regex reg = new Regex("Font '28 Days Later' has not been found. Using (.*) font instead. Reason: default font setting.");
            
            foreach (WarningInfo fontWarning in callback.FontWarnings)
            {        
                Match match = reg.Match(fontWarning.Description);
                if (match.Success)
                {
                    Assert.Pass();
                    break;
                }
            }
        }

        [Test]
        [Category("SkipMono")]
        public void SubstitutionWarnings()
        {
            Document doc = new Document(MyDir + "Rendering.docx");

            // Create a new class implementing IWarningCallback and assign it to the PdfSaveOptions class
            HandleDocumentSubstitutionWarnings callback = new HandleDocumentSubstitutionWarnings();
            doc.WarningCallback = callback;

            FontSettings fontSettings = new FontSettings();
            fontSettings.SubstitutionSettings.DefaultFontSubstitution.DefaultFontName = "Arial";
            fontSettings.SetFontsFolder(FontsDir, false);
            fontSettings.SubstitutionSettings.TableSubstitution.AddSubstitutes("Arial", "Arvo", "Slab");
            
            doc.FontSettings = fontSettings;
            doc.Save(ArtifactsDir + "Font.SubstitutionWarnings.pdf");

            Assert.AreEqual("Font \'Arial\' has not been found. Using \'Arvo\' font instead. Reason: table substitution.",
                callback.FontWarnings[0].Description);
            Assert.AreEqual("Font \'Times New Roman\' has not been found. Using \'M+ 2m\' font instead. Reason: font info substitution.",
                callback.FontWarnings[1].Description);
        }

        [Test]
        public void SubstitutionWarningsClosestMatch()
        {
            Document doc = new Document(MyDir + "Bullet points with alternative font.docx");

            // Create a new class implementing IWarningCallback and assign it to the PdfSaveOptions class
            HandleDocumentSubstitutionWarnings callback = new HandleDocumentSubstitutionWarnings();
            doc.WarningCallback = callback;

            doc.Save(ArtifactsDir + "Font.SubstitutionWarningsClosestMatch.pdf");

            Assert.True(callback.FontWarnings[0].Description
                .Equals("Font \'SymbolPS\' has not been found. Using \'Wingdings\' font instead. Reason: font info substitution."));
        }

        [Test]
        public void SetFontAutoColor()
        {
            //ExStart
            //ExFor:Font.AutoColor
            //ExSummary:Shows how to improve readability by automatically selecting text color based on the brightness of its background.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // If a run's Font object does not specify a text color,
            // it will automatically select either black or white depending on the color of the background pattern color.
            Assert.AreEqual(Color.Empty.ToArgb(), builder.Font.Color.ToArgb());

            // The default color for text is black. If the color of the background is dark, black text will be difficult to see.
            // To solve this problem, the AutoColor property will display this text in white.
            builder.Font.Shading.BackgroundPatternColor = Color.DarkBlue;

            builder.Writeln("The text color automatically chosen for this run is white.");

            Assert.AreEqual(Color.White.ToArgb(), doc.FirstSection.Body.Paragraphs[0].Runs[0].Font.AutoColor.ToArgb());

            // If we change the background to a light color, black will be a more suitable text color than white,
            // so the auto color will display this text in black.
            builder.Font.Shading.BackgroundPatternColor = Color.LightBlue;

            builder.Writeln("The text color automatically chosen for this run is black.");

            Assert.AreEqual(Color.Black.ToArgb(), doc.FirstSection.Body.Paragraphs[1].Runs[0].Font.AutoColor.ToArgb());

            doc.Save(ArtifactsDir + "Font.SetFontAutoColor.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Font.SetFontAutoColor.docx");
            Run run = doc.FirstSection.Body.Paragraphs[0].Runs[0];

            Assert.AreEqual("The text color automatically chosen for this run is white.", run.GetText().Trim());
            Assert.AreEqual(Color.Empty.ToArgb(), run.Font.Color.ToArgb());
            Assert.AreEqual(Color.DarkBlue.ToArgb(), run.Font.Shading.BackgroundPatternColor.ToArgb());

            run = doc.FirstSection.Body.Paragraphs[1].Runs[0];

            Assert.AreEqual("The text color automatically chosen for this run is black.", run.GetText().Trim());
            Assert.AreEqual(Color.Empty.ToArgb(), run.Font.Color.ToArgb());
            Assert.AreEqual(Color.LightBlue.ToArgb(), run.Font.Shading.BackgroundPatternColor.ToArgb());
        }

        //ExStart
        //ExFor:Font.Hidden
        //ExFor:Paragraph.Accept
        //ExFor:DocumentVisitor.VisitParagraphStart(Paragraph)
        //ExFor:DocumentVisitor.VisitFormField(FormField)
        //ExFor:DocumentVisitor.VisitTableEnd(Table)
        //ExFor:DocumentVisitor.VisitCellEnd(Cell)
        //ExFor:DocumentVisitor.VisitRowEnd(Row)
        //ExFor:DocumentVisitor.VisitSpecialChar(SpecialChar)
        //ExFor:DocumentVisitor.VisitGroupShapeStart(GroupShape)
        //ExFor:DocumentVisitor.VisitShapeStart(Shape)
        //ExFor:DocumentVisitor.VisitCommentStart(Comment)
        //ExFor:DocumentVisitor.VisitFootnoteStart(Footnote)
        //ExFor:SpecialChar
        //ExFor:Node.Accept
        //ExFor:Paragraph.ParagraphBreakFont
        //ExFor:Table.Accept
        //ExSummary:Shows how to use a DocumentVisitor implementation to remove all hidden content from a document.
        [Test] //ExSkip
        public void RemoveHiddenContentFromDocument()
        {
            Document doc = new Document(MyDir + "Hidden content.docx");
            Assert.AreEqual(26, doc.GetChildNodes(NodeType.Paragraph, true).Count); //ExSkip
            Assert.AreEqual(2, doc.GetChildNodes(NodeType.Table, true).Count); //ExSkip

            RemoveHiddenContentVisitor hiddenContentRemover = new RemoveHiddenContentVisitor();

            // Below are three types of fields which can accept a document visitor,
            // which will allow it to visit the accepting node, and then traverse its child nodes in a depth-first manner.
            // 1 -  Paragraph node:
            Paragraph para = (Paragraph) doc.GetChild(NodeType.Paragraph, 4, true);
            para.Accept(hiddenContentRemover);

            // 2 -  Table node:
            Table table = (Table) doc.GetChild(NodeType.Table, 0, true);
            table.Accept(hiddenContentRemover);

            // 3 -  Document node:
            doc.Accept(hiddenContentRemover);

            doc.Save(ArtifactsDir + "Font.RemoveHiddenContentFromDocument.docx");
            TestRemoveHiddenContent(new Document(ArtifactsDir + "Font.RemoveHiddenContentFromDocument.docx")); //ExSkip
        }

        /// <summary>
        /// Removes all visited nodes marked as "hidden content".
        /// </summary>
        public class RemoveHiddenContentVisitor : DocumentVisitor
        {
            /// <summary>
            /// Called when a FieldStart node is encountered in the document.
            /// </summary>
            public override VisitorAction VisitFieldStart(FieldStart fieldStart)
            {
                if (fieldStart.Font.Hidden)
                    fieldStart.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a FieldEnd node is encountered in the document.
            /// </summary>
            public override VisitorAction VisitFieldEnd(FieldEnd fieldEnd)
            {
                if (fieldEnd.Font.Hidden)
                    fieldEnd.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a FieldSeparator node is encountered in the document.
            /// </summary>
            public override VisitorAction VisitFieldSeparator(FieldSeparator fieldSeparator)
            {
                if (fieldSeparator.Font.Hidden)
                    fieldSeparator.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a Run node is encountered in the document.
            /// </summary>
            public override VisitorAction VisitRun(Run run)
            {
                if (run.Font.Hidden)
                    run.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a Paragraph node is encountered in the document.
            /// </summary>
            public override VisitorAction VisitParagraphStart(Paragraph paragraph)
            {
                if (paragraph.ParagraphBreakFont.Hidden)
                    paragraph.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a FormField is encountered in the document.
            /// </summary>
            public override VisitorAction VisitFormField(FormField formField)
            {
                if (formField.Font.Hidden)
                    formField.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a GroupShape is encountered in the document.
            /// </summary>
            public override VisitorAction VisitGroupShapeStart(GroupShape groupShape)
            {
                if (groupShape.Font.Hidden)
                    groupShape.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a Shape is encountered in the document.
            /// </summary>
            public override VisitorAction VisitShapeStart(Shape shape)
            {
                if (shape.Font.Hidden)
                    shape.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a Comment is encountered in the document.
            /// </summary>
            public override VisitorAction VisitCommentStart(Comment comment)
            {
                if (comment.Font.Hidden)
                    comment.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a Footnote is encountered in the document.
            /// </summary>
            public override VisitorAction VisitFootnoteStart(Footnote footnote)
            {
                if (footnote.Font.Hidden)
                    footnote.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when a SpecialCharacter is encountered in the document.
            /// </summary>
            public override VisitorAction VisitSpecialChar(SpecialChar specialChar)
            {
                if (specialChar.Font.Hidden)
                    specialChar.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when visiting of a Table node is ended in the document.
            /// </summary>
            public override VisitorAction VisitTableEnd(Table table)
            {
                // The content inside table cells may be flagged as hidden content, but the tables themselves cannot. 
                // If this table had nothing but hidden content, all of it would have been removed by this visitor,
                // and there would be no child nodes left.
                // Thus, we can also treat the table itself as hidden content and remove it.
                // Tables which are empty but do not have hidden content will have cells with empty paragraphs inside,
                // which will not be removed, and will remain as child nodes of this table.
                if (!table.HasChildNodes)
                    table.Remove();
                
                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when visiting of a Cell node is ended in the document.
            /// </summary>
            public override VisitorAction VisitCellEnd(Cell cell)
            {
                if (!cell.HasChildNodes && cell.ParentNode != null)
                    cell.Remove();

                return VisitorAction.Continue;
            }

            /// <summary>
            /// Called when visiting of a Row node is ended in the document.
            /// </summary>
            public override VisitorAction VisitRowEnd(Row row)
            {
                if (!row.HasChildNodes && row.ParentNode != null)
                    row.Remove();

                return VisitorAction.Continue;
            }
        }
        //ExEnd

        private void TestRemoveHiddenContent(Document doc)
        {
            Assert.AreEqual(20, doc.GetChildNodes(NodeType.Paragraph, true).Count); //ExSkip
            Assert.AreEqual(1, doc.GetChildNodes(NodeType.Table, true).Count); //ExSkip

            foreach (Node node in doc.GetChildNodes(NodeType.Any, true))
            {
                switch (node)
                {
                    case FieldStart fieldStart:
                        Assert.False(fieldStart.Font.Hidden);
                        break;
                    case FieldEnd fieldEnd:
                        Assert.False(fieldEnd.Font.Hidden);
                        break;
                    case FieldSeparator fieldSeparator:
                        Assert.False(fieldSeparator.Font.Hidden);
                        break;
                    case Run run:
                        Assert.False(run.Font.Hidden);
                        break;
                    case Paragraph paragraph:
                        Assert.False(paragraph.ParagraphBreakFont.Hidden);
                        break;
                    case FormField formField:
                        Assert.False(formField.Font.Hidden);
                        break;
                    case GroupShape groupShape:
                        Assert.False(groupShape.Font.Hidden);
                        break;
                    case Shape shape:
                        Assert.False(shape.Font.Hidden);
                        break;
                    case Comment comment:
                        Assert.False(comment.Font.Hidden);
                        break;
                    case Footnote footnote:
                        Assert.False(footnote.Font.Hidden);
                        break;
                    case SpecialChar specialChar:
                        Assert.False(specialChar.Font.Hidden);
                        break;
                }
            } 
        }

        [Test]
        public void DefaultFonts()
        {
            //ExStart
            //ExFor:Fonts.FontInfoCollection.Contains(String)
            //ExFor:Fonts.FontInfoCollection.Count
            //ExSummary:Shows info about the fonts that are present in the blank document.
            Document doc = new Document();

            // A blank document contains 3 default fonts. Each font in the document
            // will have a corresponding FontInfo object which contains details about that font.
            Assert.AreEqual(3, doc.FontInfos.Count);

            Assert.True(doc.FontInfos.Contains("Times New Roman"));
            Assert.AreEqual(204, doc.FontInfos["Times New Roman"].Charset);

            Assert.True(doc.FontInfos.Contains("Symbol"));
            Assert.True(doc.FontInfos.Contains("Arial"));
            //ExEnd
        }

        [Test]
        public void ExtractEmbeddedFont()
        {
            //ExStart
            //ExFor:Fonts.EmbeddedFontFormat
            //ExFor:Fonts.EmbeddedFontStyle
            //ExFor:Fonts.FontInfo.GetEmbeddedFont(EmbeddedFontFormat,EmbeddedFontStyle)
            //ExFor:Fonts.FontInfo.GetEmbeddedFontAsOpenType(EmbeddedFontStyle)
            //ExFor:Fonts.FontInfoCollection.Item(Int32)
            //ExFor:Fonts.FontInfoCollection.Item(String)
            //ExSummary:Shows how to extract an embedded font from a document, and save it to the local file system.
            Document doc = new Document(MyDir + "Embedded font.docx");

            FontInfo embeddedFont = doc.FontInfos["Alte DIN 1451 Mittelschrift"];
            byte[] embeddedFontBytes = embeddedFont.GetEmbeddedFont(EmbeddedFontFormat.OpenType, EmbeddedFontStyle.Regular);
            Assert.IsNotNull(embeddedFontBytes); //ExSkip

            File.WriteAllBytes(ArtifactsDir + "Alte DIN 1451 Mittelschrift.ttf", embeddedFontBytes);
            
            // Embedded font formats may be different in other formats such as .doc.
            // We need to know the correct format before we can extract the font.
            doc = new Document(MyDir + "Embedded font.doc");

            Assert.IsNull(doc.FontInfos["Alte DIN 1451 Mittelschrift"].GetEmbeddedFont(EmbeddedFontFormat.OpenType, EmbeddedFontStyle.Regular));
            Assert.IsNotNull(doc.FontInfos["Alte DIN 1451 Mittelschrift"].GetEmbeddedFont(EmbeddedFontFormat.EmbeddedOpenType, EmbeddedFontStyle.Regular));

            // Also, we can convert embedded OpenType format, which comes from .doc documents, to OpenType.
            embeddedFontBytes = doc.FontInfos["Alte DIN 1451 Mittelschrift"].GetEmbeddedFontAsOpenType(EmbeddedFontStyle.Regular);

            File.WriteAllBytes(ArtifactsDir + "Alte DIN 1451 Mittelschrift.otf", embeddedFontBytes);
            //ExEnd
        }

        [Test]
        public void GetFontInfoFromFile() 
        {
            //ExStart
            //ExFor:Fonts.FontFamily
            //ExFor:Fonts.FontPitch
            //ExFor:Fonts.FontInfo.AltName
            //ExFor:Fonts.FontInfo.Charset
            //ExFor:Fonts.FontInfo.Family
            //ExFor:Fonts.FontInfo.Panose
            //ExFor:Fonts.FontInfo.Pitch
            //ExFor:Fonts.FontInfoCollection.GetEnumerator
            //ExSummary:Shows how to access and print details of each font in a document.
            Document doc = new Document(MyDir + "Document.docx");
            
            IEnumerator fontCollectionEnumerator = doc.FontInfos.GetEnumerator();
            while (fontCollectionEnumerator.MoveNext())
            {
                FontInfo fontInfo = (FontInfo)fontCollectionEnumerator.Current;
                if (fontInfo != null)
                {
                    Console.WriteLine("Font name: " + fontInfo.Name);

                    // Alt names are usually blank.
                    Console.WriteLine("Alt name: " + fontInfo.AltName);
                    Console.WriteLine("\t- Family: " + fontInfo.Family);
                    Console.WriteLine("\t- " + (fontInfo.IsTrueType ? "Is TrueType" : "Is not TrueType"));
                    Console.WriteLine("\t- Pitch: " + fontInfo.Pitch);
                    Console.WriteLine("\t- Charset: " + fontInfo.Charset);
                    Console.WriteLine("\t- Panose:");
                    Console.WriteLine("\t\tFamily Kind: " + fontInfo.Panose[0]);
                    Console.WriteLine("\t\tSerif Style: " + fontInfo.Panose[1]);
                    Console.WriteLine("\t\tWeight: " + fontInfo.Panose[2]);
                    Console.WriteLine("\t\tProportion: " + fontInfo.Panose[3]);
                    Console.WriteLine("\t\tContrast: " + fontInfo.Panose[4]);
                    Console.WriteLine("\t\tStroke Variation: " + fontInfo.Panose[5]);
                    Console.WriteLine("\t\tArm Style: " + fontInfo.Panose[6]);
                    Console.WriteLine("\t\tLetterform: " + fontInfo.Panose[7]);
                    Console.WriteLine("\t\tMidline: " + fontInfo.Panose[8]);
                    Console.WriteLine("\t\tX-Height: " + fontInfo.Panose[9]);
                }
            }
            //ExEnd

            Assert.AreEqual(new[] { 2, 15, 5, 2, 2, 2, 4, 3, 2, 4 }, doc.FontInfos["Calibri"].Panose);
            Assert.AreEqual(new[] { 2, 15, 3, 2, 2, 2, 4, 3, 2, 4 }, doc.FontInfos["Calibri Light"].Panose);
            Assert.AreEqual(new[] { 2, 2, 6, 3, 5, 4, 5, 2, 3, 4 }, doc.FontInfos["Times New Roman"].Panose);
        }

        [Test]
        public void FontSourceFile()
        {
            //ExStart
            //ExFor:Fonts.FileFontSource
            //ExFor:Fonts.FileFontSource.#ctor(String)
            //ExFor:Fonts.FileFontSource.#ctor(String, Int32)
            //ExFor:Fonts.FileFontSource.FilePath
            //ExFor:Fonts.FileFontSource.Type
            //ExFor:Fonts.FontSourceBase
            //ExFor:Fonts.FontSourceBase.Priority
            //ExFor:Fonts.FontSourceBase.Type
            //ExFor:Fonts.FontSourceType
            //ExSummary:Shows how to use a font file in the local file system as a font source.
            FileFontSource fileFontSource = new FileFontSource(MyDir + "Alte DIN 1451 Mittelschrift.ttf", 0);

            Document doc = new Document();
            doc.FontSettings = new FontSettings();
            doc.FontSettings.SetFontsSources(new FontSourceBase[] { fileFontSource });

            Assert.AreEqual(MyDir + "Alte DIN 1451 Mittelschrift.ttf", fileFontSource.FilePath);
            Assert.AreEqual(FontSourceType.FontFile, fileFontSource.Type);
            Assert.AreEqual(0, fileFontSource.Priority);
            //ExEnd
        }

        [Test]
        public void FontSourceFolder()
        {
            //ExStart
            //ExFor:Fonts.FolderFontSource
            //ExFor:Fonts.FolderFontSource.#ctor(String, Boolean)
            //ExFor:Fonts.FolderFontSource.#ctor(String, Boolean, Int32)
            //ExFor:Fonts.FolderFontSource.FolderPath
            //ExFor:Fonts.FolderFontSource.ScanSubfolders
            //ExFor:Fonts.FolderFontSource.Type
            //ExSummary:Shows how to use a local system folder which contains fonts as a font source.
            
            // Create a font source from a folder that contains font files.
            FolderFontSource folderFontSource = new FolderFontSource(FontsDir, false, 1);

            Document doc = new Document();
            doc.FontSettings = new FontSettings();
            doc.FontSettings.SetFontsSources(new FontSourceBase[] { folderFontSource });

            Assert.AreEqual(FontsDir, folderFontSource.FolderPath);
            Assert.AreEqual(false, folderFontSource.ScanSubfolders);
            Assert.AreEqual(FontSourceType.FontsFolder, folderFontSource.Type);
            Assert.AreEqual(1, folderFontSource.Priority);
            //ExEnd
        }

        [Test]
        public void FontSourceMemory()
        {
            //ExStart
            //ExFor:Fonts.MemoryFontSource
            //ExFor:Fonts.MemoryFontSource.#ctor(Byte[])
            //ExFor:Fonts.MemoryFontSource.#ctor(Byte[], Int32)
            //ExFor:Fonts.MemoryFontSource.FontData
            //ExFor:Fonts.MemoryFontSource.Type
            //ExSummary:Shows how to use a byte array with data from a font file as a font source.

            byte[] fontBytes = File.ReadAllBytes(MyDir + "Alte DIN 1451 Mittelschrift.ttf");
            MemoryFontSource memoryFontSource = new MemoryFontSource(fontBytes, 0);

            Document doc = new Document();
            doc.FontSettings = new FontSettings();
            doc.FontSettings.SetFontsSources(new FontSourceBase[] { memoryFontSource });

            Assert.AreEqual(FontSourceType.MemoryFont, memoryFontSource.Type);
            Assert.AreEqual(0, memoryFontSource.Priority);
            //ExEnd
        }

        [Test]
        public void FontSourceSystem()
        {
            //ExStart
            //ExFor:TableSubstitutionRule.AddSubstitutes(String, String[])
            //ExFor:FontSubstitutionRule.Enabled
            //ExFor:TableSubstitutionRule.GetSubstitutes(String)
            //ExFor:Fonts.FontSettings.ResetFontSources
            //ExFor:Fonts.FontSettings.SubstitutionSettings
            //ExFor:Fonts.FontSubstitutionSettings
            //ExFor:Fonts.SystemFontSource
            //ExFor:Fonts.SystemFontSource.#ctor
            //ExFor:Fonts.SystemFontSource.#ctor(Int32)
            //ExFor:Fonts.SystemFontSource.GetSystemFontFolders
            //ExFor:Fonts.SystemFontSource.Type
            //ExSummary:Shows how to access a document's system font source and set font substitutes.
            Document doc = new Document();
            doc.FontSettings = new FontSettings();

            // By default, a blank document always contains a system font source.
            Assert.AreEqual(1, doc.FontSettings.GetFontsSources().Length);

            SystemFontSource systemFontSource = (SystemFontSource)doc.FontSettings.GetFontsSources()[0];
            Assert.AreEqual(FontSourceType.SystemFonts, systemFontSource.Type);
            Assert.AreEqual(0, systemFontSource.Priority);
            
            PlatformID pid = Environment.OSVersion.Platform;
            bool isWindows = (pid == PlatformID.Win32NT) || (pid == PlatformID.Win32S) || (pid == PlatformID.Win32Windows) || (pid == PlatformID.WinCE);
            if (isWindows)
            {
                const string fontsPath = @"C:\WINDOWS\Fonts";
                Assert.AreEqual(fontsPath.ToLower(), SystemFontSource.GetSystemFontFolders().FirstOrDefault()?.ToLower());
            }

            foreach (string systemFontFolder in SystemFontSource.GetSystemFontFolders())
            {
                Console.WriteLine(systemFontFolder);
            }

            // Set a font that exists in the Windows Fonts directory as a substitute for one that doesn't.
            doc.FontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = true;
            doc.FontSettings.SubstitutionSettings.TableSubstitution.AddSubstitutes("Kreon-Regular", new[] { "Calibri" });

            Assert.AreEqual(1, doc.FontSettings.SubstitutionSettings.TableSubstitution.GetSubstitutes("Kreon-Regular").Count());
            Assert.Contains("Calibri", doc.FontSettings.SubstitutionSettings.TableSubstitution.GetSubstitutes("Kreon-Regular").ToArray());

            // Alternatively, we could add a folder font source in which the corresponding folder contains the font.
            FolderFontSource folderFontSource = new FolderFontSource(FontsDir, false);
            doc.FontSettings.SetFontsSources(new FontSourceBase[] { systemFontSource, folderFontSource });
            Assert.AreEqual(2, doc.FontSettings.GetFontsSources().Length);

            // Resetting the font sources still leaves us with the system font source as well as our substitutes.
            doc.FontSettings.ResetFontSources();

            Assert.AreEqual(1, doc.FontSettings.GetFontsSources().Length);
            Assert.AreEqual(FontSourceType.SystemFonts, doc.FontSettings.GetFontsSources()[0].Type);
            Assert.AreEqual(1, doc.FontSettings.SubstitutionSettings.TableSubstitution.GetSubstitutes("Kreon-Regular").Count());
            //ExEnd
        }

        [Test]
        public void LoadFontFallbackSettingsFromFile()
        {
            //ExStart
            //ExFor:FontFallbackSettings.Load(String)
            //ExFor:FontFallbackSettings.Save(String)
            //ExSummary:Shows how to load and save font fallback settings to/from an XML document in the local file system.
            Document doc = new Document(MyDir + "Rendering.docx");
            
            // Load an XML document which defines a set of font fallback settings.
            FontSettings fontSettings = new FontSettings();
            fontSettings.FallbackSettings.Load(MyDir + "Font fallback rules.xml");

            doc.FontSettings = fontSettings;
            doc.Save(ArtifactsDir + "Font.LoadFontFallbackSettingsFromFile.pdf");

            // Save our document's current font fallback settings as an XML document.
            doc.FontSettings.FallbackSettings.Save(ArtifactsDir + "FallbackSettings.xml");
            //ExEnd
        }

        [Test]
        public void LoadFontFallbackSettingsFromStream()
        {
            //ExStart
            //ExFor:FontFallbackSettings.Load(Stream)
            //ExFor:FontFallbackSettings.Save(Stream)
            //ExSummary:Shows how to load and save font fallback settings to/from a stream.
            Document doc = new Document(MyDir + "Rendering.docx");

            // Load an XML document which defines a set of font fallback settings.
            using (FileStream fontFallbackStream = new FileStream(MyDir + "Font fallback rules.xml", FileMode.Open))
            {
                FontSettings fontSettings = new FontSettings();
                fontSettings.FallbackSettings.Load(fontFallbackStream);

                doc.FontSettings = fontSettings;
            }

            doc.Save(ArtifactsDir + "Font.LoadFontFallbackSettingsFromStream.pdf");

            // Use a stream to save our document's current font fallback settings as an XML document.
            using (FileStream fontFallbackStream =
                new FileStream(ArtifactsDir + "FallbackSettings.xml", FileMode.Create))
            {
                doc.FontSettings.FallbackSettings.Save(fontFallbackStream);
            }
            //ExEnd

            XmlDocument fallbackSettingsDoc = new XmlDocument();
            fallbackSettingsDoc.LoadXml(File.ReadAllText(ArtifactsDir + "FallbackSettings.xml"));
            XmlNamespaceManager manager = new XmlNamespaceManager(fallbackSettingsDoc.NameTable);
            manager.AddNamespace("aw", "Aspose.Words");

            XmlNodeList rules = fallbackSettingsDoc.SelectNodes("//aw:FontFallbackSettings/aw:FallbackTable/aw:Rule", manager);

            Assert.AreEqual("0B80-0BFF", rules[0].Attributes["Ranges"].Value);
            Assert.AreEqual("Vijaya", rules[0].Attributes["FallbackFonts"].Value);

            Assert.AreEqual("1F300-1F64F", rules[1].Attributes["Ranges"].Value);
            Assert.AreEqual("Segoe UI Emoji, Segoe UI Symbol", rules[1].Attributes["FallbackFonts"].Value);

            Assert.AreEqual("2000-206F, 2070-209F, 20B9", rules[2].Attributes["Ranges"].Value);
            Assert.AreEqual("Arial", rules[2].Attributes["FallbackFonts"].Value);

            Assert.AreEqual("3040-309F", rules[3].Attributes["Ranges"].Value);
            Assert.AreEqual("MS Gothic", rules[3].Attributes["FallbackFonts"].Value);
            Assert.AreEqual("Times New Roman", rules[3].Attributes["BaseFonts"].Value);

            Assert.AreEqual("3040-309F", rules[4].Attributes["Ranges"].Value);
            Assert.AreEqual("MS Mincho", rules[4].Attributes["FallbackFonts"].Value);

            Assert.AreEqual("Arial Unicode MS", rules[5].Attributes["FallbackFonts"].Value);
        }

        [Test]
        public void LoadNotoFontsFallbackSettings()
        {
            //ExStart
            //ExFor:FontFallbackSettings.LoadNotoFallbackSettings
            //ExSummary:Shows how to add predefined font fallback settings for Google Noto fonts.
            FontSettings fontSettings = new FontSettings();

            // These are free fonts licensed under the SIL Open Font License.
            // We can download the fonts here:
            // https://www.google.com/get/noto/#sans-lgc
            fontSettings.SetFontsFolder(FontsDir + "Noto", false);

            // Note that the predefined settings only use Sans-style Noto fonts with regular weight. 
            // Some of the Noto fonts use advanced typography features.
            // Fonts featuring advanced typography may not be rendered correctly as Aspose.Words currently does not support them.
            fontSettings.FallbackSettings.LoadNotoFallbackSettings();
            fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;
            fontSettings.SubstitutionSettings.DefaultFontSubstitution.DefaultFontName = "Noto Sans";

            Document doc = new Document();
            doc.FontSettings = fontSettings;
            //ExEnd

            TestUtil.VerifyWebResponseStatusCode(HttpStatusCode.OK, "https://www.google.com/get/noto/#sans-lgc");
        }

        [Test]
        public void DefaultFontSubstitutionRule()
        {
            //ExStart
            //ExFor:Fonts.DefaultFontSubstitutionRule
            //ExFor:Fonts.DefaultFontSubstitutionRule.DefaultFontName
            //ExFor:Fonts.FontSubstitutionSettings.DefaultFontSubstitution
            //ExSummary:Shows how to set the default font substitution rule.
            Document doc = new Document();
            FontSettings fontSettings = new FontSettings();
            doc.FontSettings = fontSettings;

            // Get the default substitution rule within FontSettings.
            // This rule will substitute all missing fonts with "Times New Roman".
            DefaultFontSubstitutionRule defaultFontSubstitutionRule = fontSettings.SubstitutionSettings.DefaultFontSubstitution;
            Assert.True(defaultFontSubstitutionRule.Enabled);
            Assert.AreEqual("Times New Roman", defaultFontSubstitutionRule.DefaultFontName);

            // Set the default font substitute to "Courier New".
            defaultFontSubstitutionRule.DefaultFontName = "Courier New";

            // Using a document builder, add some text in a font that we do not have to see the substitution take place,
            // and then render the result in a PDF.
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.Font.Name = "Missing Font";
            builder.Writeln("Line written in a missing font, which will be substituted with Courier New.");

            doc.Save(ArtifactsDir + "Font.DefaultFontSubstitutionRule.pdf");
            //ExEnd

            Assert.AreEqual("Courier New", defaultFontSubstitutionRule.DefaultFontName);
        }

        [Test]
        public void FontConfigSubstitution()
        {
            //ExStart
            //ExFor:Fonts.FontConfigSubstitutionRule
            //ExFor:Fonts.FontConfigSubstitutionRule.Enabled
            //ExFor:Fonts.FontConfigSubstitutionRule.IsFontConfigAvailable
            //ExFor:Fonts.FontConfigSubstitutionRule.ResetCache
            //ExFor:Fonts.FontSubstitutionRule
            //ExFor:Fonts.FontSubstitutionRule.Enabled
            //ExFor:Fonts.FontSubstitutionSettings.FontConfigSubstitution
            //ExSummary:Shows operating system-dependent font config substitution.
            FontSettings fontSettings = new FontSettings();
            FontConfigSubstitutionRule fontConfigSubstitution = fontSettings.SubstitutionSettings.FontConfigSubstitution;

            bool isWindows = new[] { PlatformID.Win32NT, PlatformID.Win32S, PlatformID.Win32Windows, PlatformID.WinCE }
                .Any(p => Environment.OSVersion.Platform == p);

            // The FontConfigSubstitutionRule object works differently on Windows/non-Windows platforms.
            // On Windows, it is unavailable.
            if (isWindows)
            {
                Assert.False(fontConfigSubstitution.Enabled);
                Assert.False(fontConfigSubstitution.IsFontConfigAvailable());
            }

            bool isLinuxOrMac = new[] { PlatformID.Unix, PlatformID.MacOSX }.Any(p => Environment.OSVersion.Platform == p);

            // On Linux/Mac, we will have access to it, and will be able to perform operations.
            if (isLinuxOrMac)
            {
                Assert.True(fontConfigSubstitution.Enabled);
                Assert.True(fontConfigSubstitution.IsFontConfigAvailable());

                fontConfigSubstitution.ResetCache();
            }
            //ExEnd
        }

        [Test]
        public void FallbackSettings()
        {
            //ExStart
            //ExFor:Fonts.FontFallbackSettings.LoadMsOfficeFallbackSettings
            //ExFor:Fonts.FontFallbackSettings.LoadNotoFallbackSettings
            //ExSummary:Shows how to load pre-defined fallback font settings.
            Document doc = new Document();

            FontSettings fontSettings = new FontSettings();
            doc.FontSettings = fontSettings;
            FontFallbackSettings fontFallbackSettings = fontSettings.FallbackSettings;

            // Save the default fallback font scheme to an XML document.
            // For example, one of the elements has a value of "0C00-0C7F" for Range and a corresponding "Vani" value for FallbackFonts.
            // This means that if the font some text is using does not have symbols for the 0x0C00-0x0C7F Unicode block,
            // the fallback scheme will use symbols from the "Vani" font as a substitute.
            fontFallbackSettings.Save(ArtifactsDir + "Font.FallbackSettings.Default.xml");

            // Below are two pre-defined font fallback schemes we can choose from.
            // 1 -  Use the default Microsoft Office scheme, which is the same one as the default:
            fontFallbackSettings.LoadMsOfficeFallbackSettings();
            fontFallbackSettings.Save(ArtifactsDir + "Font.FallbackSettings.LoadMsOfficeFallbackSettings.xml");

            // 2 -  Use the scheme built from Google Noto fonts:
            fontFallbackSettings.LoadNotoFallbackSettings();
            fontFallbackSettings.Save(ArtifactsDir + "Font.FallbackSettings.LoadNotoFallbackSettings.xml");
            //ExEnd

            XmlDocument fallbackSettingsDoc = new XmlDocument();
            fallbackSettingsDoc.LoadXml(File.ReadAllText(ArtifactsDir + "Font.FallbackSettings.Default.xml"));
            XmlNamespaceManager manager = new XmlNamespaceManager(fallbackSettingsDoc.NameTable);
            manager.AddNamespace("aw", "Aspose.Words");

            XmlNodeList rules = fallbackSettingsDoc.SelectNodes("//aw:FontFallbackSettings/aw:FallbackTable/aw:Rule", manager);

            Assert.AreEqual("0C00-0C7F", rules[5].Attributes["Ranges"].Value);
            Assert.AreEqual("Vani", rules[5].Attributes["FallbackFonts"].Value);
        }

        [Test]
        public void FallbackSettingsCustom()
        {
            //ExStart
            //ExFor:Fonts.FontSettings.FallbackSettings
            //ExFor:Fonts.FontFallbackSettings
            //ExFor:Fonts.FontFallbackSettings.BuildAutomatic
            //ExSummary:Shows how to distribute fallback fonts across Unicode character code ranges.
            Document doc = new Document();

            FontSettings fontSettings = new FontSettings();
            doc.FontSettings = fontSettings;
            FontFallbackSettings fontFallbackSettings = fontSettings.FallbackSettings;

            // Configure our font settings to source fonts only from the "MyFonts" folder.
            FolderFontSource folderFontSource = new FolderFontSource(FontsDir, false);
            fontSettings.SetFontsSources(new FontSourceBase[] { folderFontSource });

            // Calling BuildAutomatic() will generate a fallback scheme that
            // distributes accessible fonts across as many Unicode character codes as possible.
            // In our case, it only has access to the handful of fonts inside the "MyFonts" folder.
            fontFallbackSettings.BuildAutomatic();
            fontFallbackSettings.Save(ArtifactsDir + "Font.FallbackSettingsCustom.BuildAutomatic.xml");

            // We can also load a custom substitution scheme from a file like this.
            // This scheme applies the "Arvo" font across the "0000-00ff" Unicode blocks, the "Squarish Sans CT" font across "0100-024f",
            // and the "M+ 2m" font in all other ranges that other fonts in the scheme do not cover.
            fontFallbackSettings.Load(MyDir + "Custom font fallback settings.xml");

            // Create a document builder, and set its font to one that does not exist in any of our sources.
            // Our font settings will invoke the fallback scheme for characters that we type using the unavailable font.
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.Font.Name = "Missing Font";

            // Use the builder to print every Unicode character from 0x0021 to 0x052F,
            // with descriptive lines dividing Unicode blocks we defined in our custom font fallback scheme.
            for (int i = 0x0021; i < 0x0530; i++)
            {
                switch (i)
                {
                    case 0x0021:
                        builder.Writeln("\n\n0x0021 - 0x00FF: \nBasic Latin/Latin-1 Supplement Unicode blocks in \"Arvo\" font:");
                        break;
                    case 0x0100:
                        builder.Writeln("\n\n0x0100 - 0x024F: \nLatin Extended A/B blocks, mostly in \"Squarish Sans CT\" font:");
                        break;
                    case 0x0250:
                        builder.Writeln("\n\n0x0250 - 0x052F: \nIPA/Greek/Cyrillic blocks in \"M+ 2m\" font:");
                        break;
                }

                builder.Write($"{Convert.ToChar(i)}");
            }

            doc.Save(ArtifactsDir + "Font.FallbackSettingsCustom.pdf");
            //ExEnd

            XmlDocument fallbackSettingsDoc = new XmlDocument();
            fallbackSettingsDoc.LoadXml(File.ReadAllText(ArtifactsDir + "Font.FallbackSettingsCustom.BuildAutomatic.xml"));
            XmlNamespaceManager manager = new XmlNamespaceManager(fallbackSettingsDoc.NameTable);
            manager.AddNamespace("aw", "Aspose.Words");

            XmlNodeList rules = fallbackSettingsDoc.SelectNodes("//aw:FontFallbackSettings/aw:FallbackTable/aw:Rule", manager);

            Assert.AreEqual("0000-007F", rules[0].Attributes["Ranges"].Value);
            Assert.AreEqual("Arvo", rules[0].Attributes["FallbackFonts"].Value);

            Assert.AreEqual("0180-024F", rules[3].Attributes["Ranges"].Value);
            Assert.AreEqual("M+ 2m", rules[3].Attributes["FallbackFonts"].Value);

            Assert.AreEqual("0300-036F", rules[6].Attributes["Ranges"].Value);
            Assert.AreEqual("Noticia Text", rules[6].Attributes["FallbackFonts"].Value);

            Assert.AreEqual("0590-05FF", rules[10].Attributes["Ranges"].Value);
            Assert.AreEqual("Squarish Sans CT", rules[10].Attributes["FallbackFonts"].Value);
        }

        [Test]
        public void TableSubstitutionRule()
        {
            //ExStart
            //ExFor:Fonts.TableSubstitutionRule
            //ExFor:Fonts.TableSubstitutionRule.LoadLinuxSettings
            //ExFor:Fonts.TableSubstitutionRule.LoadWindowsSettings
            //ExFor:Fonts.TableSubstitutionRule.Save(System.IO.Stream)
            //ExFor:Fonts.TableSubstitutionRule.Save(System.String)
            //ExSummary:Shows how to access font substitution tables for Windows and Linux.
            Document doc = new Document();
            FontSettings fontSettings = new FontSettings();
            doc.FontSettings = fontSettings;

            // Create a new table substitution rule, and load the default Microsoft Windows font substitution table.
            TableSubstitutionRule tableSubstitutionRule = fontSettings.SubstitutionSettings.TableSubstitution;
            tableSubstitutionRule.LoadWindowsSettings();

            // In Windows, the default substitute for the "Times New Roman CE" font is "Times New Roman".
            Assert.AreEqual(new[] { "Times New Roman" }, 
                tableSubstitutionRule.GetSubstitutes("Times New Roman CE").ToArray());

            // We can save the table in the form of an XML document.
            tableSubstitutionRule.Save(ArtifactsDir + "Font.TableSubstitutionRule.Windows.xml");

            // Linux has its own substitution table.
            // There are multiple substitute fonts for "Times New Roman CE".
            // If the first substitute, "FreeSerif" is also unavailable,
            // this rule will cycle through the others in the array until it finds one that is available.
            tableSubstitutionRule.LoadLinuxSettings();
            Assert.AreEqual(new[] { "FreeSerif", "Liberation Serif", "DejaVu Serif" }, 
                tableSubstitutionRule.GetSubstitutes("Times New Roman CE").ToArray());

            // Save the Linux substitution table in the form of an XML document using a stream.
            using (FileStream fileStream = new FileStream(ArtifactsDir + "Font.TableSubstitutionRule.Linux.xml", FileMode.Create))
            {
                tableSubstitutionRule.Save(fileStream);
            }
            //ExEnd

            XmlDocument fallbackSettingsDoc = new XmlDocument();
            fallbackSettingsDoc.LoadXml(File.ReadAllText(ArtifactsDir + "Font.TableSubstitutionRule.Windows.xml"));
            XmlNamespaceManager manager = new XmlNamespaceManager(fallbackSettingsDoc.NameTable);
            manager.AddNamespace("aw", "Aspose.Words");

            XmlNodeList rules = fallbackSettingsDoc.SelectNodes("//aw:TableSubstitutionSettings/aw:SubstitutesTable/aw:Item", manager);

            Assert.AreEqual("Times New Roman CE", rules[16].Attributes["OriginalFont"].Value);
            Assert.AreEqual("Times New Roman", rules[16].Attributes["SubstituteFonts"].Value);

            fallbackSettingsDoc.LoadXml(File.ReadAllText(ArtifactsDir + "Font.TableSubstitutionRule.Linux.xml"));
            rules = fallbackSettingsDoc.SelectNodes("//aw:TableSubstitutionSettings/aw:SubstitutesTable/aw:Item", manager);

            Assert.AreEqual("Times New Roman CE", rules[31].Attributes["OriginalFont"].Value);
            Assert.AreEqual("FreeSerif, Liberation Serif, DejaVu Serif", rules[31].Attributes["SubstituteFonts"].Value);
        }

        [Test]
        public void TableSubstitutionRuleCustom()
        {
            //ExStart
            //ExFor:Fonts.FontSubstitutionSettings.TableSubstitution
            //ExFor:Fonts.TableSubstitutionRule.AddSubstitutes(System.String,System.String[])
            //ExFor:Fonts.TableSubstitutionRule.GetSubstitutes(System.String)
            //ExFor:Fonts.TableSubstitutionRule.Load(System.IO.Stream)
            //ExFor:Fonts.TableSubstitutionRule.Load(System.String)
            //ExFor:Fonts.TableSubstitutionRule.SetSubstitutes(System.String,System.String[])
            //ExSummary:Shows how to work with custom font substitution tables.
            Document doc = new Document();
            FontSettings fontSettings = new FontSettings();
            doc.FontSettings = fontSettings;

            // Create a new table substitution rule, and load the default Windows font substitution table.
            TableSubstitutionRule tableSubstitutionRule = fontSettings.SubstitutionSettings.TableSubstitution;

            // If we select fonts exclusively from our own folder, we will need a custom substitution table.
            // We will no longer have access to the Microsoft Windows fonts,
            // such as "Arial" or "Times New Roman", since they do not exist in our new font folder.
            FolderFontSource folderFontSource = new FolderFontSource(FontsDir, false);
            fontSettings.SetFontsSources(new FontSourceBase[] { folderFontSource });

            // Below are two ways of loading a substitution table from a file in the local file system.
            // 1 -  From a stream:
            using (FileStream fileStream = new FileStream(MyDir + "Font substitution rules.xml", FileMode.Open))
            {
                tableSubstitutionRule.Load(fileStream);
            }

            // 2 -  Directly from a file:
            tableSubstitutionRule.Load(MyDir + "Font substitution rules.xml");

            // Since we no longer have access to "Arial", our font table will first try substitute it with "Nonexistent Font".
            // We don't have this font, so it will move onto the next substitute, which is "Kreon", found in the "MyFonts" folder.
            Assert.AreEqual(new[] { "Missing Font", "Kreon" }, tableSubstitutionRule.GetSubstitutes("Arial").ToArray());

            // We can expand this table programmatically. We will add an entry that substitutes "Times New Roman" with "Arvo"
            Assert.Null(tableSubstitutionRule.GetSubstitutes("Times New Roman"));
            tableSubstitutionRule.AddSubstitutes("Times New Roman", "Arvo");
            Assert.AreEqual(new[] { "Arvo" }, tableSubstitutionRule.GetSubstitutes("Times New Roman").ToArray());

            // We can add a secondary fallback substitute for an existing font entry with AddSubstitutes()
            // In case "Arvo" is unavailable, our table will look for "M+ 2m" as a second substitute option.
            tableSubstitutionRule.AddSubstitutes("Times New Roman", "M+ 2m");
            Assert.AreEqual(new[] { "Arvo", "M+ 2m" }, tableSubstitutionRule.GetSubstitutes("Times New Roman").ToArray());

            // SetSubstitutes() can set a new list of substitute fonts for a font.
            tableSubstitutionRule.SetSubstitutes("Times New Roman", new[] { "Squarish Sans CT", "M+ 2m" });
            Assert.AreEqual(new[] { "Squarish Sans CT", "M+ 2m" }, tableSubstitutionRule.GetSubstitutes("Times New Roman").ToArray());

            // Writing text in fonts that we do not have access to will invoke our substitution rules.
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.Font.Name = "Arial";
            builder.Writeln("Text written in Arial, to be substituted by Kreon.");

            builder.Font.Name = "Times New Roman";
            builder.Writeln("Text written in Times New Roman, to be substituted by Squarish Sans CT.");

            doc.Save(ArtifactsDir + "Font.TableSubstitutionRule.Custom.pdf");
            //ExEnd
        }

        [Test]
        public void ResolveFontsBeforeLoadingDocument()
        {
            //ExStart
            //ExFor:LoadOptions.FontSettings
            //ExSummary:Shows how to designate font substitutes during loading.
            LoadOptions loadOptions = new LoadOptions();
            loadOptions.FontSettings = new FontSettings();

            // Set a font substitution rule for a LoadOptions object.
            // If the document we are loading uses a font which we do not have,
            // this rule will substitute the unavailable font with one that does exist.
            // In this case, all uses of the "MissingFont" will convert to "Comic Sans MS".
            TableSubstitutionRule substitutionRule = loadOptions.FontSettings.SubstitutionSettings.TableSubstitution;
            substitutionRule.AddSubstitutes("MissingFont", new[] { "Comic Sans MS" });

            Document doc = new Document(MyDir + "Missing font.html", loadOptions);

            // At this point such text will still be in "MissingFont".
            // Font substitution will take place when we render the document.
            Assert.AreEqual("MissingFont", doc.FirstSection.Body.FirstParagraph.Runs[0].Font.Name);

            doc.Save(ArtifactsDir + "Font.ResolveFontsBeforeLoadingDocument.pdf");
            //ExEnd
        }
        
        [Test]
        public void LineSpacing()
        {
            //ExStart
            //ExFor:Font.LineSpacing
            //ExSummary:Shows how to get a font's line spacing, in points.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Set different fonts for the DocumentBuilder, and verify their line spacing.
            builder.Font.Name = "Calibri";
            Assert.AreEqual(14.6484375d, builder.Font.LineSpacing);

            builder.Font.Name = "Times New Roman";
            Assert.AreEqual(13.798828125d, builder.Font.LineSpacing);
            //ExEnd
        }

        [Test]
        public void HasDmlEffect()
        {
            //ExStart
            //ExFor:Font.HasDmlEffect(TextDmlEffect)
            //ExSummary:Shows how to check if a run displays a DrawingML text effect.
            Document doc = new Document(MyDir + "DrawingML text effects.docx");
            
            RunCollection runs = doc.FirstSection.Body.FirstParagraph.Runs;
            
            Assert.True(runs[0].Font.HasDmlEffect(TextDmlEffect.Shadow));
            Assert.True(runs[1].Font.HasDmlEffect(TextDmlEffect.Shadow));
            Assert.True(runs[2].Font.HasDmlEffect(TextDmlEffect.Reflection));
            Assert.True(runs[3].Font.HasDmlEffect(TextDmlEffect.Effect3D));
            Assert.True(runs[4].Font.HasDmlEffect(TextDmlEffect.Fill));
            //ExEnd
        }

        //ExStart
        //ExFor:StreamFontSource
        //ExFor:StreamFontSource.OpenFontDataStream
        //ExSummary:Shows how to load fonts from stream.
        [Test] //ExSkip
        public void StreamFontSourceFileRendering()
        {
            FontSettings fontSettings = new FontSettings();
            fontSettings.SetFontsSources(new FontSourceBase[] { new StreamFontSourceFile() });

            DocumentBuilder builder = new DocumentBuilder();
            builder.Document.FontSettings = fontSettings;
            builder.Font.Name = "Kreon-Regular";
            builder.Writeln("Test aspose text when saving to PDF.");

            builder.Document.Save(ArtifactsDir + "Font.StreamFontSourceFileRendering.pdf");
        }
        
        /// <summary>
        /// Load the font data only when it is required as opposed to storing it in the memory for the entire lifetime of the "FontSettings" object.
        /// </summary>
        private class StreamFontSourceFile : StreamFontSource
        {
            public override Stream OpenFontDataStream()
            {
                return File.OpenRead(FontsDir + "Kreon-Regular.ttf");
            }
        }
        //ExEnd

        [Test, Category("IgnoreOnJenkins")]
        public void CheckScanUserFontsFolder()
        {
            // On Windows 10 fonts may be installed either into system folder "%windir%\fonts" for all users
            // or into user folder "%userprofile%\AppData\Local\Microsoft\Windows\Fonts" for current user
            SystemFontSource systemFontSource = new SystemFontSource();
            Assert.NotNull(systemFontSource.GetAvailableFonts()
                    .FirstOrDefault(x => x.FilePath.Contains("\\AppData\\Local\\Microsoft\\Windows\\Fonts")),
                "Fonts did not install to the user font folder");
        }
    }
}
#endif