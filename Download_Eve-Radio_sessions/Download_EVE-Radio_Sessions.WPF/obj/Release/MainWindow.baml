   M S B A M L   `   `   `  ���� Z  VDownload_EVE-Radio_Sessions.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Download_EVE_Radio_Sessions.WPFDownload_EVE_Radio_Sessions.WPF  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 0    *Download_EVE_Radio_Sessions.WPF.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ?local-clr-namespace:Download_EVE_Radio_Sessions.WPF     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     f bGalaSoft.MvvmLight.Platform, Version=5.3.0.19032, Culture=neutral, PublicKeyToken=5f873c45e98af8a1.command http://www.galasoft.ch/mvvmlight    � Title$  
MainWindow��$	��600��$	��850�� � DataContext �� *
EVERadioVM+   Locator �� Source# [  .  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ Closing��.  (System.Windows.Interactivity.TriggerBase   Actions. /  )GalaSoft.MvvmLight.Command.EventToCommand     PassEventArgsToCommand .    Command �� *WindowClosingCommand+.���  � ColumnDefinitions �� $��* ��� $��* �.+��� -   $��everadiosessionsListView��$��
10,10,8,10q�	 �� SelectionMode$	 Multiple=����� *EVERadioSessions+���� .R�� 
 ^� ShowDuration$
 20000�� ^� HasDropShadow .  � ToolTip _� .����  �� Orientation$ Vertical=�.+���  �� ContentStringFormat$ Downloadspeed: {0}kb/s������ *DownloadSpeed+�� $) #Started downloading at: {0:Humm:ss}������ *TimeStarted+�� $ Filesize: {0}MB������ *
FileSize+.+�� $��0,-5,0,0q�.+��  � Minimum$ 0Q� � Maximum$	 100Q�$��100px�� � Value �� *
Progress+�� $��Center=�$
 {0}%������ *
Progress+ �� FallbackValue$ ?���� $��10,0,0,0q����� *
FileName+���� *Achtergrondkleur+����  �� 
TargetType" ��.���  �� Property �@��
IsSelected �� Value �� *
IsSelected+�� $��Download All��$
��Left=�$��	10,10,0,0q�$	��Top=�$��1��$��30��$��80������ *DownloadAllCommand+�� $��Download Selected��$
��Left=�$��	95,10,0,0q�$	��Top=�$��1��$��30��$	��106������  �� ElementName$ everadiosessionsListView�� �� Path$ SelectedItems����� *DownloadSelectedCommand+����  �� Triggers ��  �� Binding �� $ everadiosessionsListView��$ SelectedIndex� �� Value -1.a���  � {� .  �� $��	Use proxy��$��1��$
��Left=�$��
206,15,0,0q�$	��Top=�$��15��$��72�� d� 	IsChecked �� *
UseProxy+�� $��	Feedback:��$��1��$
��Left=�$��
16,0,0,129q�$��Bottom=�$��26��$��63���� $��1��$��
10,0,10,10q�$��Bottom=�$	��119������ *FeedbackList+���� *FeedbackColor+�� $��Choose Download Folder��$��1��$
��Left=�$��	10,45,0,0q�$	��Top=�$��20��$	��138������ *ChooseDownloadFolderCommand+�� $��Open Downloadfolder��$��1��$��
153,45,0,0q�$	��Top=�$
��Left=�$	��121��$��20������ *OpenDownloadFolderCommand+�� $��1��$��
10,69,10,0q�$	��Top=�$��26������ *DownloadFolder+�� $��1��$
��Left=�$��
10,255,0,0q�$	��Top=�$��26������ *DownloadSpeed+�� $��	Todo List��$��1��$
��Left=�$��
10,100,0,0q�$	��Top=�$��26��$��57���� $��1��$	��100��$��10,125,10,0q�$	��Top=�{�.  .@��� .��&$Show total networkspeed we are using�� .��,*Delete bad parts of song (intro and outro)�� .��)'Show amount already downloaded per song�� .��ZXImprove downloadspeed shown per item. Does it now only update when an item gets updated?�� $��Open FileZilla��$��1��$
��Left=�$��
10,230,0,0q�$	��Top=�$��75��$��20������ *OpenFilleZillaCommand+