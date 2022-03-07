using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ProjectLibrary;
namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private enum ObjectProject
        {
            Map,
            Entity,
            Prefab,
            ResourseSprite,
            Sprite,
            Animation
        }
        private Project _mainProject = new Project();
        private List<Stream> _resourseImageStreams = new List<Stream>();
        private string _entityCharacteristicsForRendering = "";
        private string _selectedSceneName = "";
        private string _selectedImage = "";
        private string _saveFilePath = "";
        public MainWindow()
        {
            InitializeComponent();
            TypeEntity typeEntity = TypeEntity.PhysicalEntity;
            foreach (var item in Enum.GetValues(typeEntity.GetType()))
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item;
                EntityTypeComboBox.Items.Add(comboBoxItem);
            }
        }
        private ImageSource ByteToImage(string path)
        {
            BitmapImage biImg = new BitmapImage();
            ImageSource imgSrc;
            using (Stream ms = File.OpenRead(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                if (File.Exists(Directory.GetCurrentDirectory() + @"\" + fileInfo.Name ))
                    File.Delete(Directory.GetCurrentDirectory() + @"\" + fileInfo.Name );
                File.Copy(path, Directory.GetCurrentDirectory() + @"\" + fileInfo.Name);
                    Stream ms2 = File.OpenRead(Directory.GetCurrentDirectory() + @"\" + fileInfo.Name);
                biImg.BeginInit();
                biImg.StreamSource = ms2;
                biImg.EndInit();
                _resourseImageStreams.Add(ms2);
                imgSrc = biImg;
            }
            return imgSrc;
        }
        private void AddObjectToPanel(ObjectProject objectProject, object[] args)
        {
            Button button = new Button();
            button.Content = args[0];
            switch (objectProject)
            {
                case ObjectProject.Map:
                    button.Click += MapButton_Click;
                    MapsWrapPanel.Children.Add(button);
                    break;
                case ObjectProject.Entity:
                    button.Click += EntityButton_Click;
                    EntitiesListBox.Items.Add(button);
                    break;
                case ObjectProject.Prefab:
                    button.Click += PrefabButton_Click;
                    PrefabsStackPanel.Children.Add(button);
                    SceneGLControl.Invalidate();
                    break;
                case ObjectProject.ResourseSprite:
                
                    ImageSource src = ByteToImage(args[1].ToString());
                    ImageWithText imageWithText = new ImageWithText(args[0].ToString(), src);
                    imageWithText.MouseDown += ImageWithText_MouseDown;
                    imageWithText.MainImage.Stretch = Stretch.Uniform;
                    ResourceSpriteWrapPanel.Children.Add(imageWithText);
                    break;
                case ObjectProject.Animation:
                    button.Click += AnimationButton_Click;
                    AnimationsStackPanel.Children.Add(button);
                    break;
                case ObjectProject.Sprite:
                    foreach (var image in ResourceSpriteWrapPanel.Children)
                    {
                        if((image as ImageWithText).ImageTextBlock.Text== args[1].ToString())
                        {
                            ImageSource source = (image as ImageWithText).MainImage.Source;
                            ImageWithText spriteWithText = new ImageWithText(args[0].ToString(), source);
                            spriteWithText.MouseDown += SpriteWithText_MouseDown;
                         
                            spriteWithText.MainImage.Stretch = Stretch.Uniform;
                           SpriteStackPanel.Children.Add(spriteWithText);
                        }
                    }
                    
                    break;
                default:
                    break;
            }
        }

        private void SpriteWithText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClearImageWithTextColor(sender as ImageWithText, false);
            _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.Animations[_mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.SelectedAnimation].SelectedSprite = (sender as ImageWithText).ImageTextBlock.Text;
            EntityGLControl.Invalidate();
        }

        private void AnimationButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedAnimationTextBlock.Text = (sender as Button).Content.ToString();
            SpriteStackPanel.Children.Clear();
            _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.SelectedAnimation = SelectedAnimationTextBlock.Text;
            foreach (var sprite in _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.Animations[SelectedAnimationTextBlock.Text].Sprites)
            {
               
                AddObjectToPanel(ObjectProject.Sprite, new object[] { sprite.Key, sprite.Value.Name });
            }
        }

        private void DeleteObjectFromProjectAndPanel(ObjectProject objectProject, string name)
        {
            try
            {
                switch (objectProject)
                {
                    case ObjectProject.Map:
                        _mainProject.Scenes.Remove(_selectedSceneName);
                        PrefabsStackPanel.Children.Clear();
                        for (int i = 0; i < MapsWrapPanel.Children.Count; i++)
                        {
                            if ((MapsWrapPanel.Children[i] as Button).Content.ToString() == _selectedSceneName)
                            {
                                _mainProject.Scenes.Remove(_selectedSceneName);
                                _selectedSceneName = "";
                                MapsWrapPanel.Children.RemoveAt(i);
                                if (i - 1 >= 0)
                                    MapButton_Click(MapsWrapPanel.Children[i - 1], null);
                                else
                                    SceneGLControl.Invalidate();
                                break;
                            }
                        }
                        break;
                    case ObjectProject.Entity:
                        for (int i = 0; i < EntitiesListBox.Items.Count; i++)
                        {
                            if ((EntitiesListBox.Items[i] as Button).Content.ToString() == name)
                            {
                                if (name == _entityCharacteristicsForRendering)
                                    ClearEntitySettings();
                                EntitiesListBox.Items.RemoveAt(i);
                              
                                _mainProject.EntitiesCharacteristics.Remove(name);
                                EntityGLControl.Invalidate();
                            }
                        }
                        break;
                    case ObjectProject.Prefab:
                        for (int i = 0; i < PrefabsStackPanel.Children.Count; i++)
                        {
                            if ((PrefabsStackPanel.Children[i] as Button).Content.ToString() == name)
                            {
                                if (name == NamePrefTextBlock.Text)
                                    ClearPrefSettings();
                                PrefabsStackPanel.Children.RemoveAt(i);
                                _mainProject.Scenes[_selectedSceneName].PrefabEntities.Remove(name);
                                SceneGLControl.Invalidate();

                            }
                        }
                        break;
                    case ObjectProject.ResourseSprite:
                        _mainProject.ResourceSprites.Remove(name);
                        for (int i = 0; i < ResourceSpriteWrapPanel.Children.Count; i++)
                        {
                            if ((ResourceSpriteWrapPanel.Children[i] as ImageWithText).ImageTextBlock.Text == name)
                            {
                                ResourceSpriteWrapPanel.Children.Remove(ResourceSpriteWrapPanel.Children[i] as ImageWithText);
                            }
                        }
                        break;
                    case ObjectProject.Sprite:
                        _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.Animations[SelectedAnimationTextBlock.Text].Sprites.Remove(name);
                        for (int i = 0; i < SpriteStackPanel.Children.Count; i++)
                        {
                            if ((SpriteStackPanel.Children[i] as ImageWithText).ImageTextBlock.Text == name)
                            {
                                SpriteStackPanel.Children.Remove(SpriteStackPanel.Children[i] as ImageWithText);
                                _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.Animations[SelectedAnimationTextBlock.Text].SelectedSprite = "";
                                
                            }
                        }
                        EntityGLControl.Invalidate();
                        break;
                    case ObjectProject.Animation:
                        _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.Animations.Remove(name);
                        
                        for (int i = 0; i < AnimationsStackPanel.Children.Count; i++)
                        {
                            if ((AnimationsStackPanel.Children[i] as Button).Content.ToString() == name)
                            {
                                _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.SelectedAnimation = "";
                                AnimationsStackPanel.Children.Remove(AnimationsStackPanel.Children[i] as Button);
                                SpriteStackPanel.Children.Clear();
                                EntityGLControl.Invalidate();

                            }
                        }
                        break;
                   
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось выполнить действие\n" + ex.Message);
            }
        }
        private void AddObjectToProject(ObjectProject objectProject, object[] args)
        {
            try
            {
                switch (objectProject)
                {
                    case ObjectProject.Map:
                        //0-name
                        //1-size
                        _mainProject.Scenes.Add(args[0].ToString(), new Scene(new Camera(float.Parse(args[1].ToString()), true)));

                        break;
                    case ObjectProject.Entity:
                        //0-name
                        _mainProject.EntitiesCharacteristics.Add(args[0].ToString(), new EntityCharacteristics(args[0].ToString()));
                        break;
                    case ObjectProject.Prefab:
                        //0-name
                        //1-EntityCharacteristics
                        PrefabEntity prefabEntity = new PrefabEntity(_mainProject.EntitiesCharacteristics[args[1].ToString()]);
                              _mainProject.Scenes[_selectedSceneName].PrefabEntities.Add(args[0].ToString(), prefabEntity);
                        break;
                    case ObjectProject.ResourseSprite:
                        //0-name
                        //1-path
                        using (var myBitmap = new System.Drawing.Bitmap(args[1].ToString()))
                        {
                            var pixels = new List<byte>(4 * myBitmap.Width * myBitmap.Height);
                            for (int y = 0; y < myBitmap.Height; y++)
                            {
                                for (int x = 0; x < myBitmap.Width; x++)
                                {
                                    pixels.Add(myBitmap.GetPixel(x, y).R);
                                    pixels.Add(myBitmap.GetPixel(x, y).G);
                                    pixels.Add(myBitmap.GetPixel(x, y).B);
                                    pixels.Add(myBitmap.GetPixel(x, y).A);
                                }
                            }
                            if (_mainProject.ResourceSprites.ContainsKey(args[0].ToString()))
                            {
                                _mainProject.ResourceSprites[args[0].ToString()].ArraySprite = pixels.ToArray();
                                _mainProject.ResourceSprites[args[0].ToString()].Height = myBitmap.Height;
                                _mainProject.ResourceSprites[args[0].ToString()].Width = myBitmap.Width;
                            }
                            else
                            _mainProject.ResourceSprites.Add(args[0].ToString(), new ResourceSprite(args[1].ToString(), pixels.ToArray(), args[0].ToString(), myBitmap.Height, myBitmap.Width));
                        }
                        break;
                    case ObjectProject.Animation:
                        //0-name
                        //1-nameEntity
                        if (_mainProject.EntitiesCharacteristics[args[1].ToString()].MyAnimator == null)
                            _mainProject.EntitiesCharacteristics[args[1].ToString()].MyAnimator = new Animator();
                        _mainProject.EntitiesCharacteristics[args[1].ToString()].MyAnimator.Animations.Add(args[0].ToString(), new Animation());
                        break;
                    case ObjectProject.Sprite:
                        //0-name
                        //1-nameSprite
                      _mainProject.EntitiesCharacteristics[NameEntityTextBox.Text].MyAnimator.Animations[SelectedAnimationTextBlock.Text].Sprites.Add(args[0].ToString(),_mainProject.ResourceSprites[args[1].ToString()]);
                        break;
                    default:
                        break;
                }
                AddObjectToPanel(objectProject, args);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось выполнить действие\n"+ex.Message);
            }
        }
      
        private void SceneWindowsFormsHost_Initialized(object sender, EventArgs e)
        {

        }

        private void SceneGLControl_Load(object sender, EventArgs e)
        {

        }

        private void SceneGLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.ClearColor(new Color4(0.6f, 0.6f, 0.6f, 1f));
            GL.Viewport(0, 0, SceneGLControl.Width, SceneGLControl.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            if (_selectedSceneName != "")
            {
                foreach (var pref in _mainProject.Scenes[_selectedSceneName].PrefabEntities)
                {
                    if (!_mainProject.EntitiesCharacteristics.ContainsValue(pref.Value.MyEntityCharacteristics) || pref.Value.MyEntityCharacteristics == null)
                    {
                        DeleteObjectFromProjectAndPanel(ObjectProject.Prefab, pref.Key);
                        continue;
                    }
                    pref.Value.Rendering(_mainProject.Scenes[_selectedSceneName].MyCamera.GetOrthoMatrix());
                }
            }
            SceneGLControl.SwapBuffers();

        }

        private void WindowsFormsHostEntity_Initialized(object sender, EventArgs e)
        {

        }

        private void EntityGLControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(new Color4(0.1f, 0.2f, 0.5f, 1f));

        }

        private void EntityGLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Viewport(0, 0, EntityGLControl.Width, EntityGLControl.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            if (_entityCharacteristicsForRendering != "")
                _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].Rendering(new Camera(2).GetOrthoMatrix(), Vector2.Zero, new Vector2(1, 1));
            if (_entityCharacteristicsForRendering != ""&&_mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator != null)
            {
                for (int i = 0; i < _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations.Count; i++)
                { 
                    for (int z = 0; z < _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations.Values.ToArray()[i].Sprites.Count; z++)
                    {

                        ResourceSprite spr = _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations.Values.ToArray()[i].Sprites.Values.ToArray()[z];
                        if (!_mainProject.ResourceSprites.ContainsKey(spr.Name))
                        {
                            _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations.Values.ToArray()[i].Sprites.Remove(_mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations.Values.ToArray()[i].Sprites.Keys.ToArray()[z]);
                            if(SelectedAnimationTextBlock.Text== _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations.Keys.ToArray()[i])
                            {
                                SpriteStackPanel.Children.RemoveAt(z);
                                _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations.Values.ToArray()[i].SelectedSprite = "";


                            }
                        }
                           

                    }
                }
            }
               
            EntityGLControl.SwapBuffers();


        }
        
       
        private void CreateMapButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPrefSettings();
            MapSettings mapSettings = new MapSettings();
            if (mapSettings.ShowDialog() == true)
            {
                AddObjectToProject(ObjectProject.Map,new object[] { mapSettings.MapNameTextBox.Text, mapSettings.SizeCameraTextBox.Text });

            }
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedSceneName = (sender as Button).Content.ToString();
            PrefabsStackPanel.Children.Clear();
            foreach (var pref in _mainProject.Scenes[_selectedSceneName].PrefabEntities)
                AddObjectToPanel(ObjectProject.Prefab, new object[] { pref.Key });

            SceneGLControl.Invalidate();
        }

        private void DeleteMapButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteObjectFromProjectAndPanel(ObjectProject.Map, (sender as Button).Name);
        }
       
       
        private void CreateEntityButton_Click(object sender, RoutedEventArgs e)
        {
            NameWindow entityNameWindow = new NameWindow("Название сущности:");
            if (entityNameWindow.ShowDialog() == true)
            {
                try
                {
                    AddObjectToProject(ObjectProject.Entity, new object[] { entityNameWindow.NameTextBox.Text });
                    
                }
                catch
                {
                    MessageBox.Show("Не удалось создать сущность");
                }
            }
        }

        private void EntityButton_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
           
            _entityCharacteristicsForRendering = button.Content.ToString();

            SpriteStackPanel.Children.Clear();
            RColor.Text = _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyColor.R.ToString().Replace(',', '.');
            GColor.Text = _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyColor.G.ToString().Replace(',', '.');
            BColor.Text = _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyColor.B.ToString().Replace(',', '.');
            SelectedAnimationTextBlock.Text = "";
            NameEntityTextBox.Text = _entityCharacteristicsForRendering;
            EntityTypeComboBox.SelectedIndex = (int)_mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyType;
            EntityGLControl.Invalidate();
            AnimationsStackPanel.Children.Clear();
            
           
            if (_mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator == null)
                return;
     //       _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations[_mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.SelectedAnimation].SelectedSprite = "";
     //       _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.SelectedAnimation = "";
            foreach (var animation in _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.Animations)
            {
                AddObjectToPanel(ObjectProject.Animation, new object[] { animation.Key });
            }
        }

        private void SaveEntitySettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Color4 colorF = new Color4(float.Parse(RColor.Text.Replace('.', ',')), float.Parse(GColor.Text.Replace('.', ',')), float.Parse(BColor.Text.Replace('.', ',')), 1);

                _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyColor = colorF;
                TypeEntity typeEntity = TypeEntity.SolidEntity;
                _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyType = (TypeEntity)Enum.Parse(typeEntity.GetType(), (EntityTypeComboBox.SelectedItem as ComboBoxItem).Content.ToString());
                EntityGLControl.Invalidate();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка");
            }

        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                WindowsFormsHostEntity.Visibility = Visibility.Hidden;
                WindowsFormsHostEntity.IsEnabled = false;
                SceneWindowsFormsHost.Visibility = Visibility.Visible;
                SceneWindowsFormsHost.IsEnabled = true;
                SceneGLControl.MakeCurrent();
            }
            else
            {
                WindowsFormsHostEntity.Visibility = Visibility.Visible;
                WindowsFormsHostEntity.IsEnabled = true;
                SceneWindowsFormsHost.Visibility = Visibility.Hidden;
                SceneWindowsFormsHost.IsEnabled = false;
                EntityGLControl.MakeCurrent();
            }


        }
       
       

        private void PrefabButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSceneName == "")
                return;
            NamePrefTextBlock.Text = (sender as Button).Content.ToString();
            XPositionOrdinateTextBox.Text = _mainProject.Scenes[_selectedSceneName].PrefabEntities[(sender as Button).Content.ToString()].Position.X.ToString().Replace(',', '.');
            YPositionOrdinateTextBox.Text = _mainProject.Scenes[_selectedSceneName].PrefabEntities[(sender as Button).Content.ToString()].Position.Y.ToString().Replace(',', '.');
            XScaleOrdinateTextBox.Text = _mainProject.Scenes[_selectedSceneName].PrefabEntities[(sender as Button).Content.ToString()].Scale.X.ToString().Replace(',', '.');
            YScaleOrdinateTextBox.Text = _mainProject.Scenes[_selectedSceneName].PrefabEntities[(sender as Button).Content.ToString()].Scale.Y.ToString().Replace(',', '.');
        }

        private void SavePrefSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSceneName == "")
                    return;
                _mainProject.Scenes[_selectedSceneName].PrefabEntities[NamePrefTextBlock.Text].Position = new Vector2(float.Parse(XPositionOrdinateTextBox.Text.Replace('.', ',').ToString()), float.Parse(YPositionOrdinateTextBox.Text.Replace('.', ',').ToString()));
                _mainProject.Scenes[_selectedSceneName].PrefabEntities[NamePrefTextBlock.Text].Scale = new Vector2(float.Parse(XScaleOrdinateTextBox.Text.Replace('.', ',').ToString()), float.Parse(YScaleOrdinateTextBox.Text.Replace('.', ',').ToString()));
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить изменения");
            }
            SceneGLControl.Invalidate();
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Universal hrundel project |*.uhp";
            if (saveFileDialog.ShowDialog() == true)
            {
                _mainProject.Save(saveFileDialog.FileName);
                _saveFilePath = saveFileDialog.FileName;
            }
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Universal hrundel project |*.uhp";
            if (openFileDialog.ShowDialog() == true)
            {
               
                foreach (var sprite in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\"))
                {
                    FileInfo fileInfo = new FileInfo(sprite);
                    if (fileInfo.Extension == ".png")
                        File.Delete(sprite);
                }
                _mainProject = Project.Load(openFileDialog.FileName);
                MapsWrapPanel.Children.Clear();
                ClearPrefSettings();
                PrefabsStackPanel.Children.Clear();
                EntitiesListBox.Items.Clear();
                foreach (var scene in _mainProject.Scenes)
                   AddObjectToPanel(ObjectProject.Map, new object[] { scene.Key });
                foreach (var entity in _mainProject.EntitiesCharacteristics)
                    AddObjectToPanel(ObjectProject.Entity, new object[] { entity.Key });
                foreach (var entity in _mainProject.ResourceSprites)
                    AddObjectToPanel(ObjectProject.ResourseSprite, new object[] { entity.Key, entity.Value.Path });
                _saveFilePath = openFileDialog.FileName;


            }
        }
       
        private void DeletePrefabButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteObjectFromProjectAndPanel(ObjectProject.Prefab, NamePrefTextBlock.Text);

        }
        private void ClearPrefSettings()
        {
            NamePrefTextBlock.Text = "";
            XPositionOrdinateTextBox.Text = "";
            YPositionOrdinateTextBox.Text = "";
            XScaleOrdinateTextBox.Text = "";
            YScaleOrdinateTextBox.Text = "";
        }
        private void ClearEntitySettings()
        {
            _entityCharacteristicsForRendering = "";
            NameEntityTextBox.Text = "";
            AnimationsStackPanel.Children.Clear();
            SpriteStackPanel.Children.Clear();
            SelectedAnimationTextBlock.Text = "";
            RColor.Text = "";
            GColor.Text = "";
            BColor.Text = "";

        }
        private void DeleteEntityButton_Click(object sender, RoutedEventArgs e)
        {
          
            DeleteObjectFromProjectAndPanel(ObjectProject.Entity, NameEntityTextBox.Text);

        }

        private void ExportFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Hrundel game resources |*.hgr";
            if (saveFileDialog.ShowDialog() == true)
            {
                _mainProject.SaveToResourse(saveFileDialog.FileName);
            }
        }
        private void ClearImageWithTextColor(ImageWithText imageWithText,bool isResourseSprite=true)
        {
            UIElementCollection uIElementCollection = isResourseSprite ? ResourceSpriteWrapPanel.Children : SpriteStackPanel.Children;
            foreach (var item in uIElementCollection)
            {
                if (imageWithText == (item as ImageWithText))
                    (item as ImageWithText).MainStackPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(250, 210, 150));
                else
                    (item as ImageWithText).MainStackPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(250, 235, 215));

            }
        }
        private void ImageWithText_MouseDown(object sender, MouseEventArgs e)
        {
            ClearImageWithTextColor(sender as ImageWithText);
            _selectedImage = (sender as ImageWithText).ImageTextBlock.Text;
        }

        private void AddSpriteButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Sprite |*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                NameWindow nameWindow = new NameWindow("Название спрайта");
                if (nameWindow.ShowDialog() == true)
                {
                    AddObjectToProject(ObjectProject.ResourseSprite, new object[] { nameWindow.NameTextBox.Text, openFileDialog.FileName });
                    
                }

            }
        }

        private void DeleteSpriteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteObjectFromProjectAndPanel(ObjectProject.ResourseSprite, _selectedImage);
        }

        private void FramewCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void FramewCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (FramewCheckBox.IsChecked == true)
            {
                FrameOrAnimationAddButton.Content = "Добавить кадр";
                FrameOrAnimationDeleteButton.Content = "Удалить кадр";
            } else
            {
                FrameOrAnimationAddButton.Content = "Добавить анимацию";
                FrameOrAnimationDeleteButton.Content = "Удалить анимацию";
            }
        }
        
        private void FrameOrAnimationAddButton_Click(object sender, RoutedEventArgs e)
        {
            if(FrameOrAnimationAddButton.Content.ToString()== "Добавить анимацию")
            {
                NameWindow nameWindow = new NameWindow("Название анимации:");
                if (nameWindow.ShowDialog() == true)
                {
                    AddObjectToProject(ObjectProject.Animation, new object[] { nameWindow.NameTextBox.Text, NameEntityTextBox.Text });
                    //   AddAnimation(nameWindow.NameTextBox.Text, _entityCharacteristicsForRendering);
                }
            }
            else
            {
                NameWindow nameWindow = new NameWindow("Название кадра:");
                if (nameWindow.ShowDialog() == true)
                {
                    NameWindow nameWindow2 = new NameWindow("Название ресурса:");
                    if (nameWindow2.ShowDialog() == true)
                    {
                        AddObjectToProject(ObjectProject.Sprite, new object[] { nameWindow.NameTextBox.Text, nameWindow2.NameTextBox.Text });
                        //   AddAnimation(nameWindow.NameTextBox.Text, _entityCharacteristicsForRendering);
                    }
                }
            }
        }

        private void AddPrefabButton_Click(object sender, RoutedEventArgs e)
        {
            NameWindow nameWindow = new NameWindow("Название префаба:");
            if (nameWindow.ShowDialog() == true)
            {
                NameWindow nameWindow2 = new NameWindow("Название сущности:");
                if (nameWindow2.ShowDialog() == true)
                {
                    AddObjectToProject(ObjectProject.Prefab, new object[] { nameWindow.NameTextBox.Text, nameWindow2.NameTextBox.Text });
                }
              
            }
          
        }

        private void FrameOrAnimationDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FrameOrAnimationDeleteButton.Content.ToString() == "Удалить кадр")
            {
                if (_mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator != null)
                    DeleteObjectFromProjectAndPanel(ObjectProject.Sprite, _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.SelectedAnimation);
            }
            else
            {
                if (_mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator != null)
                    DeleteObjectFromProjectAndPanel(ObjectProject.Animation, _mainProject.EntitiesCharacteristics[_entityCharacteristicsForRendering].MyAnimator.SelectedAnimation);
            }
        }

        private void UpdateResourceButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceSpriteWrapPanel.Children.Clear();
            foreach (var item in _resourseImageStreams)
                item.Close();
                for (int i = 0; i < _mainProject.ResourceSprites.Count; i++)
                    AddObjectToProject(ObjectProject.ResourseSprite, new object[] { _mainProject.ResourceSprites.ElementAt(i).Key, _mainProject.ResourceSprites.ElementAt(i).Value.Path });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_saveFilePath != "")
            {
                UpdateResourceButton_Click(null, null);
                _mainProject.Save(_saveFilePath);
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            foreach (var item in _resourseImageStreams)
                item.Close();
            foreach (var sprite in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\"))
            {
                FileInfo fileInfo = new FileInfo(sprite);
                if (fileInfo.Extension == ".png")
                    File.Delete(sprite);
            }
        }
    }
}
