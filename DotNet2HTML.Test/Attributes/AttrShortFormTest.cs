using DotNet2HTML.Attributes;
using DotNet2HTML.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Test.Attributes
{

    [TestClass]
    public class AttrShortFormTest
    {

        [TestMethod]
        public void AddTo_EmptyTag()
        {
            string expected = "<input id=\"some-id\" class=\"some-class\">";
            string actual = Input(Attrs("#some-id.some-class")).Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddTo_ContainerTag()
        {
            string expected = "<div id=\"some-id\" class=\"some-class\"></div>";
            string actual = Div(Attrs("#some-id.some-class")).Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddTo_JustId()
        {
            string expected = "<div id=\"some-id\"></div>";
            string actual = Div(Attrs("#some-id")).Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddTo_JustClass()
        {
            string expected = "<div class=\"some-class\"></div>";
            string actual = Div(Attrs(".some-class")).Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddTo_AddTwoClasses()
        {
            string expected = "<div class=\"some-class some-other-class\"></div>";
            string actual = Div(Attrs(".some-class.some-other-class")).Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddTo_StringWithoutIdOrClass()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                Div(Attrs("some-class")).Render();
            });
        }

        [TestMethod]
        public void AddTo_StringWithTwoIds()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                Div(Attrs("#id1#id2")).Render();
            });
        }

        [TestMethod]
        public void AddTo_StringWithStupidlyPlacedId()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                Div(Attrs("id1#id2")).Render();
            });
        }
    }
}
