﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WControls.Drawable;

namespace WControls.Digitals.Cells
{
    /// <summary>
    /// A seven segment cell to display digits
    /// </summary>
    [ToolboxItem(true)]
    [Description("A seven segment cell to display digits")]
    public partial class SevenSegmentCell : DigitalCell
    {
        protected const double PADDING_RATIO_X = .0697d;
        protected const double PADDING_RATIO_Y = .0446d;
        protected const double BARSIZE_RATIO_X = .1638d;
        protected const double BARSIZE_RATIO_Y = .1049d;
        protected const double OFFSET_RATIO_X = .0251d;
        protected const double OFFSET_RATIO_Y = .0161d;

        private Dictionary<char, bool[]> SEGMENT_MAP = new Dictionary<char, bool[]>()
        {
            {'0', new bool[7] { true, true, true, true, true, true, false } },
            {'1', new bool[7] { false, true, true, false, false, false, false } },
            {'2', new bool[7] { true, true, false, true, true, false, true } },
            {'3', new bool[7] { true, true, true, true, false, false, true } },
            {'4', new bool[7] { false, true, true, false, false, true, true } },
            {'5', new bool[7] { true, false, true, true, false, true, true } },
            {'6', new bool[7] { true, false, true, true, true, true, true } },
            {'7', new bool[7] { true, true, true, false, false, false, false } },
            {'8', new bool[7] { true, true, true, true, true, true, true } },
            {'9', new bool[7] { true, true, true, false, false, true, true } },
        };

        protected DigitalBar m_segmentA = new DigitalBar(SegmentOrientation.Horizontal);
        protected DigitalBar m_segmentB = new DigitalBar(SegmentOrientation.Vertical);
        protected DigitalBar m_segmentC = new DigitalBar(SegmentOrientation.Vertical);
        protected DigitalBar m_segmentD = new DigitalBar(SegmentOrientation.Horizontal);
        protected DigitalBar m_segmentE = new DigitalBar(SegmentOrientation.Vertical);
        protected DigitalBar m_segmentF = new DigitalBar(SegmentOrientation.Vertical);
        protected DigitalBar m_segmentG = new DigitalBar(SegmentOrientation.Horizontal);

        public SevenSegmentCell()
        {
            Value = '0';

            m_segmentA.Color = this.ForeColor;
            m_segmentB.Color = this.ForeColor;
            m_segmentC.Color = this.ForeColor;
            m_segmentD.Color = this.ForeColor;
            m_segmentE.Color = this.ForeColor;
            m_segmentF.Color = this.ForeColor;
            m_segmentG.Color = this.ForeColor;

            m_segmentA.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentB.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentC.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentD.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentE.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentF.OpacityWhenOff = this.OpacityWhenOff;
            m_segmentG.OpacityWhenOff = this.OpacityWhenOff;

            m_segmentA.Corners = SegmentCorners.BothTop;
            m_segmentB.Corners = SegmentCorners.TopRight;
            m_segmentC.Corners = SegmentCorners.BottomRight;
            m_segmentD.Corners = SegmentCorners.BothBottom;
            m_segmentE.Corners = SegmentCorners.BottomLeft;
            m_segmentF.Corners = SegmentCorners.TopLeft;
            m_segmentG.Corners = SegmentCorners.None;

            Segments.Add(m_segmentA);
            Segments.Add(m_segmentB);
            Segments.Add(m_segmentC);
            Segments.Add(m_segmentD);
            Segments.Add(m_segmentE);
            Segments.Add(m_segmentF);
            Segments.Add(m_segmentG);

            InitializeComponent();
        }

        protected override void CalculateSegments(RectangleF container)
        {
            float paddingX = (float)(PADDING_RATIO_X * container.Width);
            float paddingY = (float)(PADDING_RATIO_Y * container.Height);
            float segWidthX = (float)(BARSIZE_RATIO_X * container.Width);
            float segWidthY = (float)(BARSIZE_RATIO_Y * container.Height);
            float offsetX = (float)(OFFSET_RATIO_X * container.Width);
            float offsetY = (float)(OFFSET_RATIO_Y * container.Height);

            float vertHeight = (container.Height / 2f) - paddingY + (segWidthY / 2f);
            float horizWidth = container.Width - (2 * paddingX);
            float rightStartVert = container.Right - (paddingX + segWidthX);
            float bottomStartVert = container.Bottom - (paddingY + vertHeight);
            float bottomStartHoriz = container.Bottom - (paddingY + segWidthY);
            float middleStartHoriz = (container.Y + (container.Height / 2f)) - (segWidthY / 2f);

            RectangleF rectA = new RectangleF(container.X + paddingX, container.Y + paddingY,
                horizWidth, segWidthY);
            RectangleF rectB = new RectangleF(rightStartVert, container.Y + paddingY,
                segWidthX, vertHeight);
            RectangleF rectC = new RectangleF(rightStartVert, bottomStartVert,
                segWidthX, vertHeight);
            RectangleF rectD = new RectangleF(container.X + paddingX, bottomStartHoriz,
                horizWidth, segWidthY);
            RectangleF rectE = new RectangleF(container.X + paddingX, bottomStartVert,
                segWidthX, vertHeight);
            RectangleF rectF = new RectangleF(container.X + paddingX, container.Y + paddingY,
                segWidthX, vertHeight);
            RectangleF rectG = new RectangleF(container.X + paddingX, middleStartHoriz,
                horizWidth, segWidthY);

            m_segmentA.Padding = offsetX;
            m_segmentA.TipLength = segWidthX;
            m_segmentA.CalculatePaths(rectA);

            m_segmentB.Padding = offsetY;
            m_segmentB.TipLength = segWidthY;
            m_segmentB.CalculatePaths(rectB);

            m_segmentC.Padding = offsetY;
            m_segmentC.TipLength = segWidthY;
            m_segmentC.CalculatePaths(rectC);

            m_segmentD.Padding = offsetX;
            m_segmentD.TipLength = segWidthX;
            m_segmentD.CalculatePaths(rectD);

            m_segmentE.Padding = offsetY;
            m_segmentE.TipLength = segWidthY;
            m_segmentE.CalculatePaths(rectE);

            m_segmentF.Padding = offsetY;
            m_segmentF.TipLength = segWidthY;
            m_segmentF.CalculatePaths(rectF);

            m_segmentG.Padding = offsetX;
            m_segmentG.TipLength = segWidthX;
            m_segmentG.CalculatePaths(rectG);
        }

        protected override bool CanHandleValue(char value)
        {
            return char.IsDigit(value);
        }

        protected override void UpdateSegmentsOnOff()
        {
            bool[] segs = SEGMENT_MAP[Value];

            m_segmentA.IsOn = segs[0];
            m_segmentB.IsOn = segs[1];
            m_segmentC.IsOn = segs[2];
            m_segmentD.IsOn = segs[3];
            m_segmentE.IsOn = segs[4];
            m_segmentF.IsOn = segs[5];
            m_segmentG.IsOn = segs[6];
        }
    }
}
