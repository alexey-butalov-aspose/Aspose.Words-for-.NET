﻿// Copyright (c) 2001-2020 Aspose Pty Ltd. All Rights Reserved.
//
// This file is part of Aspose.Words. The source code in this file
// is only intended as a supplement to the documentation, and is provided
// "as is", without warranty of any kind, either expressed or implied.
//////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Words;
using Aspose.Words.Fields;
using Aspose.Words.Layout;
using Aspose.Words.Properties;
using NUnit.Framework;

namespace ApiExamples
{
    [TestFixture]
    public class ExProperties : ApiExampleBase
    {
        [Test]
        public void BuiltIn()
        {
            //ExStart
            //ExFor:BuiltInDocumentProperties
            //ExFor:Document.BuiltInDocumentProperties
            //ExFor:Document.CustomDocumentProperties
            //ExFor:DocumentProperty
            //ExFor:DocumentProperty.Name
            //ExFor:DocumentProperty.Value
            //ExFor:DocumentProperty.Type
            //ExSummary:Shows how to work with built-in document properties.
            Document doc = new Document(MyDir + "Properties.docx");

            // The "Document" object contains some of its metadata in its members.
            Console.WriteLine($"Document filename:\n\t \"{doc.OriginalFileName}\"");

            // The document also stores metadata in its built-in properties.
            // Each built-in property is a member of the document's "BuiltInDocumentProperties" object.
            Console.WriteLine("Built-in Properties:");
            foreach (DocumentProperty docProperty in doc.BuiltInDocumentProperties)
            {
                Console.WriteLine(docProperty.Name);
                Console.WriteLine($"\tType:\t{docProperty.Type}");

                // Some properties may store multiple values.
                if (docProperty.Value is Array)
                {
                    foreach (object value in docProperty.Value as Array)
                        Console.WriteLine($"\tValue:\t\"{value}\"");
                }
                else
                {
                    Console.WriteLine($"\tValue:\t\"{docProperty.Value}\"");
                }
            }
            //ExEnd

            Assert.AreEqual(28, doc.BuiltInDocumentProperties.Count);
        }

        [Test]
        public void Custom()
        {
            //ExStart
            //ExFor:BuiltInDocumentProperties.Item(String)
            //ExFor:CustomDocumentProperties
            //ExFor:DocumentProperty.ToString
            //ExFor:DocumentPropertyCollection.Count
            //ExFor:DocumentPropertyCollection.Item(int)
            //ExSummary:Shows how to work with custom document properties.
            Document doc = new Document(MyDir + "Properties.docx");

            // Every document contains a collection of custom properties, which, like the built-in properties, are key-value pairs.
            // Unlike the built-in properties, many of which the document maintains by itself, we need to create all of our own custom properties. 
            Assert.AreEqual("Value of custom document property", doc.CustomDocumentProperties["CustomProperty"].ToString());

            doc.CustomDocumentProperties.Add("CustomProperty2", "Value of custom document property #2");

            Console.WriteLine("Custom Properties:");
            foreach (var customDocumentProperty in doc.CustomDocumentProperties)
            {
                Console.WriteLine(customDocumentProperty.Name);
                Console.WriteLine($"\tType:\t{customDocumentProperty.Type}");
                Console.WriteLine($"\tValue:\t\"{customDocumentProperty.Value}\"");
            }
            //ExEnd

            Assert.AreEqual(2, doc.CustomDocumentProperties.Count);
        }

        [Test]
        public void Description()
        {
            //ExStart
            //ExFor:BuiltInDocumentProperties.Author
            //ExFor:BuiltInDocumentProperties.Category
            //ExFor:BuiltInDocumentProperties.Comments
            //ExFor:BuiltInDocumentProperties.Keywords
            //ExFor:BuiltInDocumentProperties.Subject
            //ExFor:BuiltInDocumentProperties.Title
            //ExSummary:Shows how to work with built-in document properties in the "Description" category.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            BuiltInDocumentProperties properties = doc.BuiltInDocumentProperties;

            // Below are four built-in document properties that have fields that can display their values in the document body.
            // 1 -  "Author" property, which we can display using an AUTHOR field:
            properties.Author = "John Doe";
            builder.Write("Author:\t");
            builder.InsertField(FieldType.FieldAuthor, true);

            // 2 -  "Title" property, which we can display using a TITLE field:
            properties.Title = "John's Document";
            builder.Write("\nDoc title:\t");
            builder.InsertField(FieldType.FieldTitle, true);

            // 3 -  "Subject" property, which we can display using a SUBJECT field:
            properties.Subject = "My subject";
            builder.Write("\nSubject:\t");
            builder.InsertField(FieldType.FieldSubject, true);

            // 4 -  "Comments" property, which we can display using a COMMENTS field:
            properties.Comments = $"This is {properties.Author}'s document about {properties.Subject}";
            builder.Write("\nComments:\t\"");
            builder.InsertField(FieldType.FieldComments, true);
            builder.Write("\"");

            // The "Category" built-in property does not have a field that can display its value.
            properties.Category = "My category";

            // We can set multiple keywords for a document by separating the string value of the "Keywords" property with semicolons.
            properties.Keywords = "Tag 1; Tag 2; Tag 3";

            // We can right-click this document in Windows Explorer and find these properties in "Properties" -> "Details".
            // The "Author" built-in property is in the "Origin" group, and the others are in the "Description" group.
            doc.Save(ArtifactsDir + "Properties.Description.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Properties.Description.docx");

            properties = doc.BuiltInDocumentProperties;

            Assert.AreEqual("John Doe", properties.Author);
            Assert.AreEqual("My category", properties.Category);
            Assert.AreEqual($"This is {properties.Author}'s document about {properties.Subject}", properties.Comments);
            Assert.AreEqual("Tag 1; Tag 2; Tag 3", properties.Keywords);
            Assert.AreEqual("My subject", properties.Subject);
            Assert.AreEqual("John's Document", properties.Title);
            Assert.AreEqual("Author:\t\u0013 AUTHOR \u0014John Doe\u0015\r" +
                            "Doc title:\t\u0013 TITLE \u0014John's Document\u0015\r" +
                            "Subject:\t\u0013 SUBJECT \u0014My subject\u0015\r" +
                            "Comments:\t\"\u0013 COMMENTS \u0014This is John Doe's document about My subject\u0015\"", doc.GetText().Trim());
        }

        [Test]
        public void Origin()
        {
            //ExStart
            //ExFor:BuiltInDocumentProperties.Company
            //ExFor:BuiltInDocumentProperties.CreatedTime
            //ExFor:BuiltInDocumentProperties.LastPrinted
            //ExFor:BuiltInDocumentProperties.LastSavedBy
            //ExFor:BuiltInDocumentProperties.LastSavedTime
            //ExFor:BuiltInDocumentProperties.Manager
            //ExFor:BuiltInDocumentProperties.NameOfApplication
            //ExFor:BuiltInDocumentProperties.RevisionNumber
            //ExFor:BuiltInDocumentProperties.Template
            //ExFor:BuiltInDocumentProperties.TotalEditingTime
            //ExFor:BuiltInDocumentProperties.Version
            //ExSummary:Shows how to work with document properties in the "Origin" category.
            // Open a document that we have created and edited using Microsoft Word.
            Document doc = new Document(MyDir + "Properties.docx");
            BuiltInDocumentProperties properties = doc.BuiltInDocumentProperties;

            // The following built-in properties contain information regarding the creation and editing of this document.
            // We can right-click this document in Windows Explorer and find
            // these properties via "Properties" -> "Details" -> "Origin" category.
            // Fields such as PRINTDATE and EDITTIME can display these values in the document body.
            Console.WriteLine($"Created using {properties.NameOfApplication}, on {properties.CreatedTime}");
            Console.WriteLine($"Minutes spent editing: {properties.TotalEditingTime}");
            Console.WriteLine($"Date/time last printed: {properties.LastPrinted}");
            Console.WriteLine($"Template document: {properties.Template}");

            // We can also change the values of built-in properties.
            properties.Company = "Doe Ltd.";
            properties.Manager = "Jane Doe";
            properties.Version = 5;
            properties.RevisionNumber++;

            // Microsoft Word updates the following properties automatically when we save the document.
            // To use these properties with Aspose.Words, we will need to set values for them manually.
            properties.LastSavedBy = "John Doe";
            properties.LastSavedTime = DateTime.Now;

            // We can right-click this document in Windows Explorer and find these properties in "Properties" -> "Details" -> "Origin".
            doc.Save(ArtifactsDir + "Properties.Origin.docx");
            //ExEnd

            properties = new Document(ArtifactsDir + "Properties.Origin.docx").BuiltInDocumentProperties;

            Assert.AreEqual("Doe Ltd.", properties.Company);
            Assert.AreEqual(new DateTime(2006, 4, 25, 10, 10, 0), properties.CreatedTime);
            Assert.AreEqual(new DateTime(2019, 4, 21, 10, 0, 0), properties.LastPrinted);
            Assert.AreEqual("John Doe", properties.LastSavedBy);
            TestUtil.VerifyDate(DateTime.Now, properties.LastSavedTime, TimeSpan.FromSeconds(5));
            Assert.AreEqual("Jane Doe", properties.Manager);
            Assert.AreEqual("Microsoft Office Word", properties.NameOfApplication);
            Assert.AreEqual(12, properties.RevisionNumber);
            Assert.AreEqual("Normal", properties.Template);
            Assert.AreEqual(8, properties.TotalEditingTime);
            Assert.AreEqual(786432, properties.Version);
        }

        //ExStart
        //ExFor:BuiltInDocumentProperties.Bytes
        //ExFor:BuiltInDocumentProperties.Characters
        //ExFor:BuiltInDocumentProperties.CharactersWithSpaces
        //ExFor:BuiltInDocumentProperties.ContentStatus
        //ExFor:BuiltInDocumentProperties.ContentType
        //ExFor:BuiltInDocumentProperties.Lines
        //ExFor:BuiltInDocumentProperties.LinksUpToDate
        //ExFor:BuiltInDocumentProperties.Pages
        //ExFor:BuiltInDocumentProperties.Paragraphs
        //ExFor:BuiltInDocumentProperties.Words
        //ExSummary:Shows how to work with document properties in the "Content" category.
        [Test] //ExSkip
        public void Content()
        {
            Document doc = new Document(MyDir + "Paragraphs.docx");
            BuiltInDocumentProperties properties = doc.BuiltInDocumentProperties;

            // By using built in properties,
            // we can treat document statistics such as word/page/character counts as metadata that can be glanced at without opening the document
            // These properties are accessed by right-clicking the file in Windows Explorer and navigating to Properties > Details > Content
            // If we want to display this data inside the document, we can use fields such as NUMPAGES, NUMWORDS, NUMCHARS etc.
            // Also, these values can also be viewed in Microsoft Word by navigating File > Properties > Advanced Properties > Statistics
            // Page count: The PageCount attribute shows the page count in real time and its value can be assigned to the Pages property

            // The "Pages" property stores the page count of the document. 
            Assert.AreEqual(6, properties.Pages);

            // The "Words", "Characters", and "CharactersWithSpaces" built-in properties also display various document statistics,
            // but we need to call the "UpdateWordCount" method on the whole document before we can expect them to contain accurate values.
            Assert.AreEqual(1054, properties.Words); //ExSkip
            Assert.AreEqual(6009, properties.Characters); //ExSkip
            Assert.AreEqual(7049, properties.CharactersWithSpaces); //ExSkip
            doc.UpdateWordCount();

            Assert.AreEqual(1035, properties.Words);
            Assert.AreEqual(6026, properties.Characters);
            Assert.AreEqual(7041, properties.CharactersWithSpaces);

            // Count the number of lines in the document, and then assign the result to the "Lines" built-in property.
            LineCounter lineCounter = new LineCounter(doc);
            properties.Lines = lineCounter.GetLineCount();

            Assert.AreEqual(142, properties.Lines);

            // Assign the number of Paragraph nodes in the document to the "Paragraphs" built-in property.
            properties.Paragraphs = doc.GetChildNodes(NodeType.Paragraph, true).Count;
            Assert.AreEqual(29, properties.Paragraphs);

            // Get an estimate of the file size of our document via the "Bytes" built-in property.
            Assert.AreEqual(20310, properties.Bytes);

            // Set a different template for our document, and then update the "Template" built-in property manually to reflect this change.
            doc.AttachedTemplate = MyDir + "Business brochure.dotx";

            Assert.AreEqual("Normal", properties.Template);    
            
            properties.Template = doc.AttachedTemplate;

            // "ContentStatus" is a descriptive built-in property.
            properties.ContentStatus = "Draft";

            // Upon saving, the "ContentType" built-in property will contain the MIME type of the output save format.
            Assert.AreEqual(string.Empty, properties.ContentType);

            // If the document contains links, and they are all up to date, we can set the "LinksUpToDate" property to "true".
            Assert.False(properties.LinksUpToDate);

            doc.Save(ArtifactsDir + "Properties.Content.docx");
            TestContent(new Document(ArtifactsDir + "Properties.Content.docx")); //ExSkip
        }

        /// <summary>
        /// Counts the lines in a document.
        /// Traverses the document's layout entities tree upon construction,
        /// counting entities of the "Line" type that also contain real text.
        /// </summary>
        private class LineCounter
        {
            public LineCounter(Document doc)
            {
                mLayoutEnumerator = new LayoutEnumerator(doc);

                CountLines();
            }

            public int GetLineCount()
            {
                return mLineCount;
            }

            private void CountLines()
            {
                do
                {
                    if (mLayoutEnumerator.Type == LayoutEntityType.Line)
                    {
                        mScanningLineForRealText = true;
                    }

                    if (mLayoutEnumerator.MoveFirstChild())
                    {
                        if (mScanningLineForRealText && mLayoutEnumerator.Kind.StartsWith("TEXT"))
                        {
                            mLineCount++;
                            mScanningLineForRealText = false;
                        }
                        CountLines();
                        mLayoutEnumerator.MoveParent();
                    }
                } while (mLayoutEnumerator.MoveNext());
            }

            private readonly LayoutEnumerator mLayoutEnumerator;
            private int mLineCount;
            private bool mScanningLineForRealText;
        }
        //ExEnd

        private void TestContent(Document doc)
        {
            BuiltInDocumentProperties properties = doc.BuiltInDocumentProperties;

            Assert.AreEqual(6, properties.Pages);

            Assert.AreEqual(1035, properties.Words);
            Assert.AreEqual(6026, properties.Characters);
            Assert.AreEqual(7041, properties.CharactersWithSpaces);
            Assert.AreEqual(142, properties.Lines);
            Assert.AreEqual(29, properties.Paragraphs);
            Assert.AreEqual(15500, properties.Bytes, 200);
            Assert.AreEqual(MyDir.Replace("\\\\", "\\") + "Business brochure.dotx", properties.Template);
            Assert.AreEqual("Draft", properties.ContentStatus);
            Assert.AreEqual(string.Empty, properties.ContentType);
            Assert.False(properties.LinksUpToDate);
        }

        [Test]
        public void Thumbnail()
        {
            //ExStart
            //ExFor:BuiltInDocumentProperties.Thumbnail
            //ExFor:DocumentProperty.ToByteArray
            //ExSummary:Shows how to append a thumbnail to an Epub document.
            // Create a blank document and add some text with a DocumentBuilder
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.Writeln("Hello world!");

            // The thumbnail property resides in a document's built-in properties, but is used exclusively by Epub e-book documents
            BuiltInDocumentProperties properties = doc.BuiltInDocumentProperties;

            // Load an image from our file system into a byte array
            byte[] thumbnailBytes = File.ReadAllBytes(ImageDir + "Logo.jpg");

            // Set the value of the Thumbnail property to the array from above
            properties.Thumbnail = thumbnailBytes;

            // Our thumbnail should be visible at the start of the document, before the text we added
            doc.Save(ArtifactsDir + "Properties.Thumbnail.epub");

            // We can also extract a thumbnail property into a byte array and then into the local file system like this
            DocumentProperty thumbnail = doc.BuiltInDocumentProperties["Thumbnail"];
            File.WriteAllBytes(ArtifactsDir + "Properties.Thumbnail.gif", thumbnail.ToByteArray());
            //ExEnd

            using (FileStream imgStream = new FileStream(ArtifactsDir + "Properties.Thumbnail.gif", FileMode.Open))
            {
                TestUtil.VerifyImage(400, 400, imgStream);
            }
        }

        [Test]
        public void HyperlinkBase()
        {
            //ExStart
            //ExFor:BuiltInDocumentProperties.HyperlinkBase
            //ExSummary:Shows how to store the base part of a hyperlink in the document's properties.
            // Create a blank document and a DocumentBuilder
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a relative hyperlink to "Document.docx", which will open that document when clicked on
            builder.InsertHyperlink("Relative hyperlink", "Document.docx", false);

            // If we do not have a "Document.docx" in the same folder as the document we are about to save, we will end up with a broken link
            Assert.False(File.Exists(ArtifactsDir + "Document.docx"));
            doc.Save(ArtifactsDir + "Properties.HyperlinkBase.BrokenLink.docx");

            // We could keep prepending something like "C:\users\...\data" to every hyperlink we place to remedy this
            // Alternatively, if we know that all our linked files will come from the same folder,
            // we could set a base hyperlink in the document properties, keeping our hyperlinks short
            BuiltInDocumentProperties properties = doc.BuiltInDocumentProperties;
            properties.HyperlinkBase = MyDir;

            Assert.True(File.Exists(properties.HyperlinkBase + ((FieldHyperlink)doc.Range.Fields[0]).Address));

            doc.Save(ArtifactsDir + "Properties.HyperlinkBase.WorkingLink.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Properties.HyperlinkBase.BrokenLink.docx");
            properties = doc.BuiltInDocumentProperties;

            Assert.AreEqual(string.Empty, properties.HyperlinkBase);

            doc = new Document(ArtifactsDir + "Properties.HyperlinkBase.WorkingLink.docx");
            properties = doc.BuiltInDocumentProperties;

            Assert.AreEqual(MyDir, properties.HyperlinkBase);
            Assert.True(File.Exists(properties.HyperlinkBase + ((FieldHyperlink)doc.Range.Fields[0]).Address));
        }

        [Test]
        public void HeadingPairs()
        {
            //ExStart
            //ExFor:Properties.BuiltInDocumentProperties.HeadingPairs
            //ExFor:Properties.BuiltInDocumentProperties.TitlesOfParts
            //ExSummary:Shows the relationship between HeadingPairs and TitlesOfParts properties.
            // Open a document that contains entries in the HeadingPairs/TitlesOfParts properties
            Document doc = new Document(MyDir + "Heading pairs and titles of parts.docx");
            
            // We can find the combined values of these collections in File > Properties > Advanced Properties > Contents tab
            // The HeadingPairs property is a collection of <string, int> pairs that determines
            // how many document parts a heading spans over
            object[] headingPairs = doc.BuiltInDocumentProperties.HeadingPairs;

            // The TitlesOfParts property contains the names of parts that belong to the above headings
            string[] titlesOfParts = doc.BuiltInDocumentProperties.TitlesOfParts;

            int headingPairsIndex = 0;
            int titlesOfPartsIndex = 0;
            while (headingPairsIndex < headingPairs.Length)
            {
                Console.WriteLine($"Parts for {headingPairs[headingPairsIndex++]}:");
                int partsCount = Convert.ToInt32(headingPairs[headingPairsIndex++]);

                for (int i = 0; i < partsCount; i++)
                    Console.WriteLine($"\t\"{titlesOfParts[titlesOfPartsIndex++]}\"");
            }
            //ExEnd

            // There are 6 array elements designating 3 heading/part count pairs
            Assert.AreEqual(6, headingPairs.Length);
            Assert.AreEqual("Title", headingPairs[0].ToString());
            Assert.AreEqual("1", headingPairs[1].ToString());
            Assert.AreEqual("Heading 1", headingPairs[2].ToString());
            Assert.AreEqual("5", headingPairs[3].ToString());
            Assert.AreEqual("Heading 2", headingPairs[4].ToString());
            Assert.AreEqual("2", headingPairs[5].ToString());

            Assert.AreEqual(8, titlesOfParts.Length);
            // "Title"
            Assert.AreEqual("", titlesOfParts[0]);
            // "Heading 1"
            Assert.AreEqual("Part1", titlesOfParts[1]);
            Assert.AreEqual("Part2", titlesOfParts[2]);
            Assert.AreEqual("Part3", titlesOfParts[3]);
            Assert.AreEqual("Part4", titlesOfParts[4]);
            Assert.AreEqual("Part5", titlesOfParts[5]);
            // "Heading 2"
            Assert.AreEqual("Part6", titlesOfParts[6]);
            Assert.AreEqual("Part7", titlesOfParts[7]);
        }

        [Test]
        public void Security()
        {
            //ExStart
            //ExFor:Properties.BuiltInDocumentProperties.Security
            //ExFor:Properties.DocumentSecurity
            //ExSummary:Shows how to use document properties to display the security level of a document.
            Document doc = new Document();

            // The "Security" property serves as a description of the security level of a document
            Assert.AreEqual(DocumentSecurity.None, doc.BuiltInDocumentProperties.Security);

            // Upon saving a document after setting its security level, Aspose automatically updates this property to the appropriate value
            doc.WriteProtection.ReadOnlyRecommended = true;
            doc.Save(ArtifactsDir + "Properties.Security.ReadOnlyRecommended.docx");

            // Open a document and verify its security level
            Assert.AreEqual(DocumentSecurity.ReadOnlyRecommended, 
                new Document(ArtifactsDir + "Properties.Security.ReadOnlyRecommended.docx").BuiltInDocumentProperties.Security);

            // Create a new document and set it to Write-Protected
            doc = new Document();

            Assert.False(doc.WriteProtection.IsWriteProtected);
            doc.WriteProtection.SetPassword("MyPassword");
            Assert.True(doc.WriteProtection.ValidatePassword("MyPassword"));
            Assert.True(doc.WriteProtection.IsWriteProtected);
            doc.Save(ArtifactsDir + "Properties.Security.ReadOnlyEnforced.docx");
            
            // This document's security level counts as "ReadOnlyEnforced" 
            Assert.AreEqual(DocumentSecurity.ReadOnlyEnforced,
                new Document(ArtifactsDir + "Properties.Security.ReadOnlyEnforced.docx").BuiltInDocumentProperties.Security);

            // Since this is still a descriptive property, we can protect a document and pick a suitable value ourselves
            doc = new Document();

            doc.Protect(ProtectionType.AllowOnlyComments, "MyPassword");
            doc.BuiltInDocumentProperties.Security = DocumentSecurity.ReadOnlyExceptAnnotations;
            doc.Save(ArtifactsDir + "Properties.Security.ReadOnlyExceptAnnotations.docx");

            Assert.AreEqual(DocumentSecurity.ReadOnlyExceptAnnotations,
                new Document(ArtifactsDir + "Properties.Security.ReadOnlyExceptAnnotations.docx").BuiltInDocumentProperties.Security);
            //ExEnd
        }

        [Test]
        public void CustomNamedAccess()
        {
            //ExStart
            //ExFor:DocumentPropertyCollection.Item(String)
            //ExFor:CustomDocumentProperties.Add(String,DateTime)
            //ExFor:DocumentProperty.ToDateTime
            //ExSummary:Shows how to create a custom document property with the value of a date and time.
            Document doc = new Document();

            doc.CustomDocumentProperties.Add("AuthorizedDate", DateTime.Now);

            Console.WriteLine($"Document authorized on {doc.CustomDocumentProperties["AuthorizedDate"].ToDateTime()}");
            //ExEnd

            TestUtil.VerifyDate(DateTime.Now, 
                DocumentHelper.SaveOpen(doc).CustomDocumentProperties["AuthorizedDate"].ToDateTime(), 
                TimeSpan.FromSeconds(1));
        }

        [Test]
        public void LinkCustomDocumentPropertiesToBookmark()
        {
            //ExStart
            //ExFor:CustomDocumentProperties.AddLinkToContent(String, String)
            //ExFor:DocumentProperty.IsLinkToContent
            //ExFor:DocumentProperty.LinkSource
            //ExSummary:Shows how to link a custom document property to a bookmark.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.StartBookmark("MyBookmark");
            builder.Write("MyBookmark contents.");
            builder.EndBookmark("MyBookmark");

            // Add linked to content property
            CustomDocumentProperties customProperties = doc.CustomDocumentProperties;
            DocumentProperty customProperty = customProperties.AddLinkToContent("Bookmark", "MyBookmark");

            // Check whether the property is linked to content
            Assert.AreEqual(true, customProperty.IsLinkToContent);
            Assert.AreEqual("MyBookmark", customProperty.LinkSource);
            Assert.AreEqual("MyBookmark contents.", customProperty.Value);

            doc.Save(ArtifactsDir + "Properties.LinkCustomDocumentPropertiesToBookmark.docx");
            //ExEnd

            doc = new Document(ArtifactsDir + "Properties.LinkCustomDocumentPropertiesToBookmark.docx");
            customProperty = doc.CustomDocumentProperties["Bookmark"];

            Assert.AreEqual(true, customProperty.IsLinkToContent);
            Assert.AreEqual("MyBookmark", customProperty.LinkSource);
            Assert.AreEqual("MyBookmark contents.", customProperty.Value);
        }

        [Test]
        public void DocumentPropertyCollection()
        {
            //ExStart
            //ExFor:CustomDocumentProperties.Add(String,String)
            //ExFor:CustomDocumentProperties.Add(String,Boolean)
            //ExFor:CustomDocumentProperties.Add(String,int)
            //ExFor:CustomDocumentProperties.Add(String,DateTime)
            //ExFor:CustomDocumentProperties.Add(String,Double)
            //ExFor:DocumentProperty.Type
            //ExFor:Properties.DocumentPropertyCollection
            //ExFor:Properties.DocumentPropertyCollection.Clear
            //ExFor:Properties.DocumentPropertyCollection.Contains(System.String)
            //ExFor:Properties.DocumentPropertyCollection.GetEnumerator
            //ExFor:Properties.DocumentPropertyCollection.IndexOf(System.String)
            //ExFor:Properties.DocumentPropertyCollection.RemoveAt(System.Int32)
            //ExFor:Properties.DocumentPropertyCollection.Remove
            //ExFor:PropertyType
            //ExSummary:Shows how to add custom properties to a document.
            Document doc = new Document();
            CustomDocumentProperties properties = doc.CustomDocumentProperties;

            // The custom property collection will be empty by default
            Assert.AreEqual(0, properties.Count);

            // We can populate it with key/value pairs with a variety of value types
            properties.Add("Authorized", true);
            properties.Add("Authorized By", "John Doe");
            properties.Add("Authorized Date", DateTime.Today);
            properties.Add("Authorized Revision", doc.BuiltInDocumentProperties.RevisionNumber);
            properties.Add("Authorized Amount", 123.45);

            // Custom properties are automatically sorted in alphabetic order
            Assert.AreEqual(1, properties.IndexOf("Authorized Amount"));
            Assert.AreEqual(5, properties.Count);

            // Enumerate and print all custom properties
            using (IEnumerator<DocumentProperty> enumerator = properties.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    Console.WriteLine($"Name: \"{enumerator.Current.Name}\"\n\tType: \"{enumerator.Current.Type}\"\n\tValue: \"{enumerator.Current.Value}\"");
            }

            // We can view/edit custom properties by opening the document and looking in File > Properties > Advanced Properties > Custom
            doc.Save(ArtifactsDir + "Properties.DocumentPropertyCollection.docx");

            // We can remove elements from the property collection by index or by name
            properties.RemoveAt(1);
            Assert.False(properties.Contains("Authorized Amount"));
            Assert.AreEqual(4, properties.Count);

            properties.Remove("Authorized Revision");
            Assert.False(properties.Contains("Authorized Revision"));
            Assert.AreEqual(3, properties.Count);

            // We can also empty the entire custom property collection at once
            properties.Clear();
            Assert.AreEqual(0, properties.Count);
            //ExEnd
        }

        [Test]
        public void PropertyTypes()
        {
            //ExStart
            //ExFor:DocumentProperty.ToBool
            //ExFor:DocumentProperty.ToInt
            //ExFor:DocumentProperty.ToDouble
            //ExFor:DocumentProperty.ToString
            //ExFor:DocumentProperty.ToDateTime
            //ExSummary:Shows various type conversion methods of custom document properties.
            Document doc = new Document();
            CustomDocumentProperties properties = doc.CustomDocumentProperties;

            DateTime authDate = DateTime.Today;
            properties.Add("Authorized", true);
            properties.Add("Authorized By", "John Doe");
            properties.Add("Authorized Date", authDate);
            properties.Add("Authorized Revision", doc.BuiltInDocumentProperties.RevisionNumber);
            properties.Add("Authorized Amount", 123.45);

            Assert.AreEqual(true, properties["Authorized"].ToBool());
            Assert.AreEqual("John Doe", properties["Authorized By"].ToString());
            Assert.AreEqual(authDate, properties["Authorized Date"].ToDateTime());
            Assert.AreEqual(1, properties["Authorized Revision"].ToInt());
            Assert.AreEqual(123.45d, properties["Authorized Amount"].ToDouble());
            //ExEnd
        }
    }
}