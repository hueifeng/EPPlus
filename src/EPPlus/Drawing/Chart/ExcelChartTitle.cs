/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  01/27/2020         EPPlus Software AB       Initial release EPPlus 5
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OfficeOpenXml.Drawing.Interfaces;
using OfficeOpenXml.Drawing.Style.Effect;
using OfficeOpenXml.Drawing.Style.ThreeD;
using OfficeOpenXml.Style;

namespace OfficeOpenXml.Drawing.Chart
{
    /// <summary>
    /// The title of a chart
    /// </summary>
    public class ExcelChartTitle : XmlHelper, IDrawingStyle, IStyleMandatoryProperties
    {
        ExcelChart _chart;
        internal ExcelChartTitle(ExcelChart chart, XmlNamespaceManager nameSpaceManager, XmlNode node) :
            base(nameSpaceManager, node)
        {
            XmlNode topNode = node.SelectSingleNode("c:title", NameSpaceManager);
            if (topNode == null)
            {
                topNode = node.OwnerDocument.CreateElement("c", "title", ExcelPackage.schemaChart);
                node.InsertBefore(topNode, node.ChildNodes[0]);
                topNode.InnerXml = GetInitXml();
            }
            _chart = chart;
            TopNode = topNode;
            AddSchemaNodeOrder(new string[] { "tx", "bodyPr", "lstStyle", "layout", "p", "overlay", "spPr", "txPr" }, ExcelDrawing._schemaNodeOrderSpPr);
            if (chart.StyleManager.StylePart != null)
            {
                chart.StyleManager.ApplyStyle(this, chart.StyleManager.Style.Title);
            }
        }

        private string GetInitXml()
        {
            return $"<c:tx><c:rich><a:bodyPr rot=\"0\" spcFirstLastPara=\"1\" vertOverflow=\"ellipsis\" vert=\"horz\" wrap=\"square\" anchor=\"ctr\" anchorCtr=\"1\" />" +
                    $"<a:lstStyle />" +
                    $"<a:p><a:pPr>" +
                    $"<a:defRPr sz=\"1080\" b=\"1\" i=\"0\" u=\"none\" strike=\"noStrike\" kern=\"1200\" baseline=\"0\">" +
                    $"<a:solidFill><a:schemeClr val=\"dk1\"/></a:solidFill><a:effectLst/><a:latin typeface=\"+mn-lt\"/><a:ea typeface=\"+mn-ea\"/><a:cs typeface=\"+mn-cs\"/></a:defRPr>" +
                    $"</a:pPr><a:r><a:t /></a:r></a:p></c:rich></c:tx><c:layout /><c:overlay val=\"0\" />" +
                    $"<c:spPr><a:noFill/><a:ln><a:noFill/></a:ln><a:effectLst/></c:spPr>";
        }

        const string titlePath = "c:tx/c:rich/a:p/a:r/a:t";
        /// <summary>
        /// The text
        /// </summary>
        public string Text
        {
            get
            {
                return RichText.Text;
            }
            set
            {
                RichText.Text = value;
            }
        }
        ExcelDrawingBorder _border = null;
        /// <summary>
        /// A reference to the border properties
        /// </summary>
        public ExcelDrawingBorder Border
        {
            get
            {
                if (_border == null)
                {
                    _border = new ExcelDrawingBorder(_chart, NameSpaceManager, TopNode, "c:spPr/a:ln", SchemaNodeOrder);
                }
                return _border;
            }
        }
        ExcelDrawingFill _fill = null;
        /// <summary>
        /// A reference to the fill properties
        /// </summary>
        public ExcelDrawingFill Fill
        {
            get
            {
                if (_fill == null)
                {
                    
                    _fill = new ExcelDrawingFill(_chart, NameSpaceManager, TopNode, "c:spPr", SchemaNodeOrder);
                }
                return _fill;
            }
        }
        ExcelTextFont _font=null;
        /// <summary>
        /// A reference to the font properties
        /// </summary>
        public ExcelTextFont Font
        {
            get
            {
                if (_font == null)
                {
                    if (_richText == null || _richText.Count == 0)
                    {
                        RichText.Add("");
                    }
                    _font = new ExcelTextFont(_chart, NameSpaceManager, TopNode, "c:tx/c:rich/a:p/a:pPr/a:defRPr", SchemaNodeOrder);
                }
                return _font;
            }
        }
        ExcelTextBody _textBody = null;
        /// <summary>
        /// Access to text body properties
        /// </summary>
        public ExcelTextBody TextBody
        {
            get
            {
                if (_textBody == null)
                {
                    _textBody = new ExcelTextBody(NameSpaceManager, TopNode, "c:tx/c:rich/a:bodyPr", SchemaNodeOrder);
                }
                return _textBody;
            }
        }
        ExcelDrawingEffectStyle _effect = null;
        /// <summary>
        /// Effects
        /// </summary>
        public ExcelDrawingEffectStyle Effect
        {
            get
            {
                if (_effect == null)
                {
                    _effect = new ExcelDrawingEffectStyle(_chart, NameSpaceManager, TopNode, "c:spPr/a:effectLst", SchemaNodeOrder);
                }
                return _effect;
            }
        }
        ExcelDrawing3D _threeD = null;
        /// <summary>
        /// 3D properties
        /// </summary>
        public ExcelDrawing3D ThreeD
        {
            get
            {
                if (_threeD == null)
                {
                    _threeD = new ExcelDrawing3D(NameSpaceManager, TopNode, "c:spPr", SchemaNodeOrder);
                }
                return _threeD;
            }
        }
        void IDrawingStyleBase.CreatespPr()
        {
            CreatespPrNode();
        }

        ExcelParagraphCollection _richText = null;
        /// <summary>
        /// Richtext
        /// </summary>
        public ExcelParagraphCollection RichText
        {
            get
            {
                if (_richText == null)
                {
                    _richText = new ExcelParagraphCollection(_chart, NameSpaceManager, TopNode, "c:tx/c:rich/a:p", SchemaNodeOrder);
                }
                return _richText;
            }
        }
        /// <summary>
        /// Show without overlaping the chart.
        /// </summary>
        public bool Overlay
        {
            get
            {
                return GetXmlNodeBool("c:overlay/@val");
            }
            set
            {
                SetXmlNodeBool("c:overlay/@val", value);
            }
        }
        /// <summary>
        /// The centering of the text. Centers the text to the smallest possible text container.
        /// </summary>
        public bool AnchorCtr
        {
            get
            {
                return GetXmlNodeBool("c:tx/c:rich/a:bodyPr/@anchorCtr", false);
            }
            set
            {
                SetXmlNodeBool("c:tx/c:rich/a:bodyPr/@anchorCtr", value, false);
            }
        }
        /// <summary>
        /// How the text is anchored
        /// </summary>
        public eTextAnchoringType Anchor
        {
            get
            {
                return GetXmlNodeString("c:tx/c:rich/a:bodyPr/@anchor").TranslateTextAchoring();
            }
            set
            {
                SetXmlNodeString("c:tx/c:rich/a:bodyPr/@anchorCtr", value.TranslateTextAchoringText());
            }
        }
        const string TextVerticalPath = "xdr:sp/xdr:txBody/a:bodyPr/@vert";
        /// <summary>
        /// Vertical text
        /// </summary>
        public eTextVerticalType TextVertical
        {
            get
            {
                return GetXmlNodeString("c:tx/c:rich/a:bodyPr/@vert").TranslateTextVertical();
            }
            set
            {
                SetXmlNodeString("c:tx/c:rich/a:bodyPr/@vert", value.TranslateTextVerticalText());
            }
        }
        /// <summary>
        /// Rotation in degrees (0-360)
        /// </summary>
        public double Rotation
        {
            get
            {
                var i=GetXmlNodeInt("c:tx/c:rich/a:bodyPr/@rot");
                if (i < 0)
                {
                    return 360 - (i / 60000);
                }
                else
                {
                    return (i / 60000);
                }
            }
            set
            {
                int v;
                if(value <0 || value > 360)
                {
                    throw(new ArgumentOutOfRangeException("Rotation must be between 0 and 360"));
                }

                if (value > 180)
                {
                    v = (int)((value - 360) * 60000);
                }
                else
                {
                    v = (int)(value * 60000);
                }
                SetXmlNodeString("c:tx/c:rich/a:bodyPr/@rot", v.ToString());
            }
        }

        void IStyleMandatoryProperties.SetMandatoryProperties()
        {
            TextBody.Anchor = eTextAnchoringType.Center;
            TextBody.AnchorCenter = true;
            TextBody.WrapText = eTextWrappingType.Square;
            TextBody.VerticalTextOverflow = eTextVerticalOverflow.Ellipsis;
            TextBody.ParagraphSpacing = true;
            TextBody.Rotation = 0;

            if (Font.Kerning == 0) Font.Kerning = 12;
            Font.Bold = Font.Bold; //Must be set

            CreatespPrNode();
        }
    }
}
