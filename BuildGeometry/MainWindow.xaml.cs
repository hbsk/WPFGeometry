using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using Microsoft.Win32;

namespace BuildGeometry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ConstAngleRad45 = Math.PI / 4.0;
        private const double ConstAngleRad90 = Math.PI / 2.0;

        // start angle of the camera
        private double camTheta = ConstAngleRad45; // 45 deg
        private double camPhi = ConstAngleRad45; // 45 deg

        private double camThetaInc = 0.05;
        private double camPhiInc = 0.05;

        Point firstMousePt;
        Boolean bMouseDown = false;

        double totalWidth;
        double totalHeight;

//        private Point3D cent = new Point3D(0, 0, 0);
        private Point3D geomCenter = new Point3D(0, 0, 0);

        private RotateTransform3D rotateByXOnly;
        private RotateTransform3D rotateByYOnly;
        private RotateTransform3D rotateByZOnly;
        private TranslateTransform3D translateXYZ;
        private ScaleTransform3D scaleXYZ;
        private Transform3DGroup trsGrp;

        IGeometryData geomData = null;


        public Point3D DisplayCenter
        {
            get
            {
                return (Point3D)GetValue(DPointProperty);
            }
            set
            {
                SetValue(DPointProperty, value);
            }
        }

        private Point3D initialPoint = new Point3D(0, 0, 85);

        // property bound to the 'Center' in the UI
        public static readonly DependencyProperty DPointProperty =
        DependencyProperty.Register("DisplayCenter", typeof(Point3D),
            typeof(MainWindow), new PropertyMetadata(new Point3D(0, 0, 85.0)));

        public Point3D GeomCenter
        {
            get
            {
                Transform3D tr3d = MyGeometry.Transform;
                Point3D trpt = tr3d.Transform(geomCenter);
                return trpt;
            }
            set
            {
                geomCenter = value;
            }
        }

        // property for camera position
        private Point3D cameraPos;
        public Point3D CameraPos
        {
            get
            {
                return cameraPos;
            }
            set
            {
                cameraPos = value;
            }
        }

        public MainWindow()
        {
            CameraPos = new Point3D(200, 200, 200);

            string[] args = Environment.GetCommandLineArgs();
            if ( args.Length < 2 )
            {
                MessageBox.Show("No input file specified...exiting");
                Application.Current.Shutdown();
            }

            string filename = args[1];

            //string filename = "C:\\Users\\sanjay\\Documents\\binary_block.stl";
            FileReader fReader = FileFormatFactory.GetFileReader(filename);
            if (fReader != null)
                geomData = fReader.getParsedData();

            if (geomData == null)
            {
                MessageBox.Show("Not able to get a data parser..");
                Application.Current.Shutdown();
            }

            InitializeComponent();

            // Transform Group to handle translations and rotations
            trsGrp = new Transform3DGroup();

            // handles translation in X, Y and Z axes
            translateXYZ = new TranslateTransform3D(0, 0, 0);

            // handles the rotation about X, Y and Z axes..
            rotateByXOnly = new RotateTransform3D();
            rotateByYOnly = new RotateTransform3D();
            rotateByZOnly = new RotateTransform3D();

            // handles scaling in X, Y and Z
            scaleXYZ = new ScaleTransform3D(1, 1, 1);

            trsGrp.Children.Add(scaleXYZ);
            trsGrp.Children.Add(rotateByXOnly);
            trsGrp.Children.Add(rotateByYOnly);
            trsGrp.Children.Add(rotateByZOnly);
            trsGrp.Children.Add(translateXYZ);

            MyGeometry.Transform = trsGrp;
        }

        // property for positions to be set in the mesh
        public Point3DCollection GeometryPositions
        {
            get
            {
                Point3DCollection points = geomData.getPositions();
                return points;
            }
        }

        // property for triangle indices to be set in the mesh
        public Int32Collection GeometryIndices
        {
            get
            {
                Int32Collection indices = geomData.getTriangleIndices();
                return indices;
            }
        }

        // handles mouse wheel zoom in/zoom out
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point3D origin = GeomCenter;
            Vector3D vec = new Vector3D(cameraPos.X, cameraPos.Y, cameraPos.Z);
            double radius = vec.Length;
            double perc = 1.0 - ((2.0 * e.Delta / 100D) / radius);

            Point3D newPoint = new Point3D(CameraPos.X * perc, CameraPos.Y * perc, CameraPos.Z * perc);
            myCamera.Position = newPoint;
            CameraPos = newPoint;
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Left:
                    {
                        camTheta -= camThetaInc;
                        break;
                    }
                case Key.Right:
                    {
                        camTheta += camThetaInc;
                        break;
                    }
                case Key.Down:
                    {
                        camPhi -= camPhiInc;
                        if (camPhi < -ConstAngleRad90)
                            camPhi = -ConstAngleRad90;
                        break;
                    }
                case Key.Up:
                    {
                        camPhi += camPhiInc;
                        if (camPhi > ConstAngleRad90)
                            camPhi = ConstAngleRad90;
                        break;
                    }
            }
            updateCameraPosition();
        }

        private void updateCameraPosition()
        {
            Vector3D vec = new Vector3D(CameraPos.X, CameraPos.Y, CameraPos.Z);

            double rad = vec.Length;
            double newZ = rad * Math.Sin(camPhi);
            double newX = rad * Math.Cos(camPhi) * Math.Cos(camTheta);
            double newY = rad * Math.Cos(camPhi) * Math.Sin(camTheta);

            //Point3D newPoint = new Point3D(newX, newY, newZ);
            CameraPos = new Point3D(newX, newY, newZ);
            myCamera.Position = CameraPos;
            myCamera.LookDirection = new Vector3D(-CameraPos.X, -CameraPos.Y, -CameraPos.Z);
            myCamera.UpDirection = new Vector3D(0, 0, 1);
        }

        private void TranslateX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            translateXYZ.OffsetX = (int)e.NewValue;
            updateDisplayCenter();
        }

        private void TranslateY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            translateXYZ.OffsetY= (int)e.NewValue;
            updateDisplayCenter();
        }

        private void TranslateZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            translateXYZ.OffsetZ = (int)e.NewValue;
            updateDisplayCenter();
        }

        private void RotateXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), e.NewValue);
            rotateByXOnly.Rotation = axis;
            updateDisplayCenter();
        }

        private void RotateYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(0, 1, 0), e.NewValue);
            rotateByYOnly.Rotation = axis;
            updateDisplayCenter();
        }

        private void RotateZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(0, 0, 1), e.NewValue);
            rotateByZOnly.Rotation = axis;
            updateDisplayCenter();
        }

        private void ScaleX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (scaleXYZ != null)
                scaleXYZ.ScaleX = e.NewValue;
            updateDisplayCenter();
        }

        private void ScaleY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (scaleXYZ != null)
                scaleXYZ.ScaleY = e.NewValue;
            updateDisplayCenter();
        }

        private void scaleZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (scaleXYZ != null)
                scaleXYZ.ScaleZ = e.NewValue;
            updateDisplayCenter();
        }

        private void updateDisplayCenter()
        {
            Point3D newpt = new Point3D();
            newpt = initialPoint;

            Transform3D tr3d = MyGeometry.Transform;
            newpt = tr3d.Transform(newpt);
            newpt.X = (int)newpt.X;
            newpt.Y = (int)newpt.Y;
            newpt.Z = (int)newpt.Z;

            DisplayCenter = newpt;
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bMouseDown = true;
            firstMousePt = e.GetPosition(this);
        }

        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!bMouseDown)
                return;

            Point secondMousePt = e.GetPosition(this);

            double dx = (secondMousePt.X - firstMousePt.X);
            double dy = (secondMousePt.Y - firstMousePt.Y);

            double camThetaInc2 = 3 * dx / totalWidth * ConstAngleRad90;
            double camPhiInc2 = 3 * dy / totalHeight * ConstAngleRad90;

            camTheta -= camThetaInc2;
            camPhi += camPhiInc2;

            if (camPhi < -ConstAngleRad90)
                camPhi = -ConstAngleRad90;
            else if (camPhi > ConstAngleRad90)
                camPhi = ConstAngleRad90;

            updateCameraPosition();
            firstMousePt = secondMousePt;
        }

        private void window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bMouseDown = false;
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            totalWidth = Viewport.ActualWidth;
            totalHeight = Viewport.ActualHeight;
        }
    }
}
