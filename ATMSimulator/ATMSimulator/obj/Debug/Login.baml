   M S B A M L   `   `   `  ����G  CATMSimulator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ATMSimulatorATMSimulator  5   	       ATMSimulator.Login   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:ATMSimulator     5   	     � Title$  Login��5   	   $	��493��6   $	��517��6$   .��5	      � #��� ��6   .+�5
   
   �� $"��Welcome to the ATM Simulator��6   $��Center=�67   $��0,31,0,0q�6[   $	��Top=�6m   $��18(�6�   �� 5   
   $��Login��6   $��Center=�6    $��	0,123,0,0q�6D   $	��Top=�6W   $��18(�6o   �� 5   
   $��Name:��6   $
��Left=�6    $��132,179,0,0q�6;   $	��Top=�6P   $��16(�6h   $��29��6v   �� 5   
   $
��PIN:��6   $
��Left=�6   $��132,234,0,0q�6:   $	��Top=�6O   $��16(�6g   $��56��6u   $��29��6�   �� 5   
   -   5         ��Name$ name_textBox��5      $
��Left=�6(   $��29��6C   $��203,179,0,0q�6O    �� TextWrapping$
 Wrap=�6d   $�� ��6x   $	��Top=�6�   $	��151��6�   $��16(�6�   G� 5   
   -   5         G�Name$ pin_textBox��5      $
��Left=�6+   $��203,234,0,0q�6F   $	��Top=�6[   $	��151��6s   $��29��6    J� RenderTransformOrigin$ 0.517,0.069&�6�   �� 5   
   -   5         ��Name$ 	login_btn��5      $��Login��6$   $��Center=�64   $��203,296,202,0q�6Q   $	��Top=�6h   $	��104��6�   $��41��6�   $��16(�6�   6�   �� 5   
   -   5         ��Name$ disableLogin_lb��5      $<��6Login is temporarialy disable, please try again later.��6)   $y�Hidden=�6j   $��Center=�6~   $��	0,339,0,0q�6�   $	��Top=�6�   ���	#FFF90C0C6�   �� 5   
   -   5        $ outOfService_lb��5      $@��:ATM is temporarily out of service. Please try again later.��6)   $y�Hidden=�6n   $��0,80,0,0q�6�   $��Center=�6�   $	��Top=�6�   ���	#FFE9F3186�   5      5      