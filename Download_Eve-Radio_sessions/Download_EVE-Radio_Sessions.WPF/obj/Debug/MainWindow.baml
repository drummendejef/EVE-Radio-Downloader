   M S B A M L   `   `   `  ����Z  VDownload_EVE-Radio_Sessions.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Download_EVE_Radio_Sessions.WPFDownload_EVE_Radio_Sessions.WPF  5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	   0    *Download_EVE_Radio_Sessions.WPF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?local-clr-namespace:Download_EVE_Radio_Sessions.WPF     5   	   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   	   f bGalaSoft.MvvmLight.Platform, Version=5.3.0.19032, Culture=neutral, PublicKeyToken=5f873c45e98af8a1.command http://www.galasoft.ch/mvvmlight  5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��850��6)    � DataContext 5
   	   �� *
EVERadioVM+   Locator �� Source# [  .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Closing��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      /  )GalaSoft.MvvmLight.Command.EventToCommand     PassEventArgsToCommand .    Command 6%   �� *WindowClosingCommand+6N   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��everadiosessionsListView��5      $��
10,10,8,10q�63   	 �� SelectionMode$	 Multiple=�6p   ��6G   �� *EVERadioSessions+��5      �� 5      .R�5      � 
 ^� ShowDuration$
 20000��6     ^� HasDropShadow .  � ToolTip 5      _� 5      .��5   "   ��  �� Orientation$ Vertical=�6-   .+�5   &   ��  �� ContentStringFormat$ Downloadspeed: {0}kb/s��6N   ��6,   �� *DownloadSpeed+6}   �� 5    &   $) #Started downloading at: {0:Humm:ss}��6L   ��6,   �� *TimeStarted+6�   �� 5!   &   $ Filesize: {0}MB��6I   ��6,   �� *
FileSize+6q   5"   #   5#      .+�5%      � $��0,-5,0,0q�6   .+�5&      �  � Minimum$ 0Q�6*    � Maximum$	 100Q�66   $��100px��6D    � Value 6R   �� *
Progress+�� 5'      $��Center=�6$   $
 {0}%��6o   ��6A   �� *
Progress+ �� FallbackValue$ ?��6�   5(      �� 5)      $��10,0,0,0q�6$   ��66   �� *
FileName+��6P   �� *Achtergrondkleur+5*      5+      ��5-      �� 5.       �� 
TargetType" ��6   .�5/      ��  �� Property �@��
IsSelected �� Value 63   �� *
IsSelected+50      52      �� 54   
   $��Download All��6   $
��Left=�6O   $��	10,10,0,0q�6j   $	��Top=�6}   $��1��6�   $��30��6�   $��80��6�   ��6(   �� *DownloadAllCommand+6�   �� 55   
   $��Download Selected��6   $
��Left=�6�   $��	95,10,0,0q�6�   $	��Top=�6�   $��1��6�   $��30��6  $	��106��6  ��6-   ��  �� ElementName$ everadiosessionsListView�� �� Path$ SelectedItems���6�   �� *DownloadSelectedCommand+��56      �� 57       �� Triggers 58      �� 59       �� Binding 6&   �� $ everadiosessionsListView��$ SelectedIndex� �� Value -16s   .a�5:      ��  � {� .  6A   5;      5=      5?      �� 5@   
   $��	Use proxy��6   $��1��6'   $
��Left=�67   $��
206,15,0,0q�6R   $	��Top=�6f   $��15��6�   $��72��6�    d� 	IsChecked 6~   �� *
UseProxy+6�   �� 5B   
   $��	Feedback:��6   $��1��6$   $
��Left=�64   $��
16,0,0,129q�6O   $��Bottom=�6c   $��26��6~   $��63��6�   �� 5C   
   $��1��6   $��
10,0,10,10q�6"   $��Bottom=�66   $	��119��6Q   ��6^   �� *FeedbackList+��6�   �� *FeedbackColor+�� 5D   
   $��Choose Download Folder��6   $��1��62   $
��Left=�6B   $��	10,45,0,0q�6]   $	��Top=�6p   $��20��6�   $	��138��6�   ��6�   �� *ChooseDownloadFolderCommand+6�   �� 5E   
   $��Open Downloadfolder��6   $��1��6/   $��
153,45,0,0q�6?   $	��Top=�6S   $
��Left=�6k   $	��121��6�   $��20��6�   ��6�   �� *OpenDownloadFolderCommand+6�   �� 5F   
   $��1��63   $��
10,69,10,0q�6C   $	��Top=�6W   $��26��6o   ��6   �� *DownloadFolder+6o   �� 5G   
   $��1��62   $
��Left=�6B   $��
10,255,0,0q�6]   $	��Top=�6q   $��26��6�   ��6   �� *DownloadSpeed+6�   �� 5J   
   $��	Todo List��6   $��1��6$   $
��Left=�64   $��
10,100,0,0q�6O   $	��Top=�6c   $��26��6{   $��57��6�   �� 5K   
   $��1��6   $	��100��6"   $��10,125,10,0q�6/   $	��Top=�6D   {�.  .@�5L      �� .��6   &$Show total networkspeed we are using6@   �� 5M      .��6   ,*Delete bad parts of song (intro and outro)6F   �� 5N      .��6   )'Show amount already downloaded per song6C   �� 5O      .��6   ZXImprove downloadspeed shown per item. Does it now only update when an item gets updated?6t   5P      �� 5Q   
   $��Open FileZilla��6   $��1��6*   $
��Left=�6:   $��
10,230,0,0q�6U   $	��Top=�6i   $��75��6�   $��20��6�   ��6�   �� *OpenFilleZillaCommand+6�   5R      5S      