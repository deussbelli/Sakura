   M S B A M L   `   `   `  ����R  NMathematical cryptology, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:Mathematical_cryptology_VigenereCipher&Mathematical_cryptology_VigenereCipher  5   	   ;    5Mathematical_cryptology_VigenereCipher.VigenereCipher   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Flocal4clr-namespace:Mathematical_cryptology_VigenereCipher     5   	     � Title$-  'Mathematical cryptology: VigenereCipher��5   	   $
��1080��69   $
��1920��6G    � 	Resources 5
       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name   CustomTextBoxStyle1  CustomTextBoxStyle2  CustomTextBoxStyle3  Encrypt �� Triggers	 P� Property
 P� Value  Decrypt  TopBottomStyle   ReadFromConsoleCheckBoxStyle  FilePathCheckBoxStyle  CustomGridStyle �� Triggers 	 ToggleButtonStyle 
 ToggleButtonStyle2  CustomListBoxStyle  CustomListBoxItemStyle �� Padding  CustomTextBlockStyle  CustomButtonStyle��%�*  '��       &	  �    &	 �    &	 �    &	 �    &	 X    &	     &	 �    &	 v    '�� +    &	 i    &		 �    &	
     &	 x     &	 �#    &	 \&    &	 �'    �� 5   
   " ��6   .�5      ��  � ��$	 200��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 20q�6'   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       � �� �	#9021127A6+   �� 5       � �� �#21127A6,   �� 5       � ��$ 1q�60   �� 5       � ��
 �����6+   �� 5       � ��$ Times New Roman)�6+   �� 5       � ��$ 14(�6)   �� 5       � ��$ 5q�6(   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 5��5      ._�5       �� $ PART_ContentHost��66   $��0q�6+   66   5!      5"      5$      5%      �� 5(   
   " ��6,   .�5)      ��  � ��$	 146��6&   �� 5*       � ��$ 28��6'   �� 5+       � ��$ 10,-100,0,0q�6'   �� 5,       � ��$ Center=�64   �� 5-       � ��$ Center=�62   �� 5.       � �� �	#9021127A6+   �� 5/       � �� �#21127A6,   �� 50       � ��$ 1q�60   �� 51       � ��
 �����6+   �� 52       � ��$ Times New Roman)�6+   �� 53       � ��$ 14(�6)   �� 54       � ��$ 5q�6(   �� 55       � �� 56      �� 57      " ��6&   .R�58      �� #��z��6!   #��z��59      #��z��5:      $ 5��5;      ._�5<      �� $ PART_ContentHost��66   $��0q�6+   66   5=      5>      5@      5A      �� 5D   
   " ��6,   .�5E      ��  � ��$	 146��6&   �� 5F       � ��$ 28��6'   �� 5G       � ��$ -420,15,0,0q�6'   �� 5H       � ��$ Center=�64   �� 5I       � ��$ Center=�62   �� 5J       � �� �	#9021127A6+   �� 5K       � �� �#21127A6,   �� 5L       � ��$ 1q�60   �� 5M       � ��
 �����6+   �� 5N       � ��$ Times New Roman)�6+   �� 5O       � ��$ 14(�6)   �� 5P       � ��$ 5q�6(   �� 5Q       � �� 5R      �� 5S      " ��6&   .R�5T      �� #��z��6!   #��z��5U      #��z��5V      $ 5��5W      ._�5X      �� $ PART_ContentHost��66   $��0q�6+   66   5Y      5Z      5\      5]      �� 5`   
   " ��6,   .�5a      ��  � ��$	 144��6&   �� 5b       � ��$ 28��6'   �� 5c       � ��$ -420,15,0,0q�6'   �� 5d       � ��$ Center=�64   �� 5e       � ��$ Center=�62   �� 5f       � �� �	#9021127A6+   �� 5g       � �� �#21127A6,   �� 5h       � ��$ 1q�60   �� 5i       � ��
 �����6+   �� 5j       � ��$ Times New Roman)�6+   �� 5k       � ��$ 14(�6)   �� 5l       � ��$ 5q�6(   �� 5m       � �� 5n      �� 5o      " ��6&   .R�5p      �� #��z��6!   #��z��5q      #��z��5r      $ 5��5s      ._�5t      �� $ PART_ContentHost��66   $��0q�6+   66   5u      5v      5x      5y      �� 5|   
   " ��6    .�5}      ��  � ��$ 14(�6)   �� 5~       � ��
 �����6+   �� 5       � �� �	#99C197E16+   �� 5�       � ��$ 1q�60   �� 5�       � ��
 ���� 6,   �� 5�       � ��$ 10q�6(   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� #��z��6!   #��z��5�      #��z��5�      $ 5��5�      $��18��6)   $	��136��65   ._�5�      �� $��Center=�6/   $��Center=�6L   5�       5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6;   . �5�   "   ��  � �� �	#995A00A16?   5�      5�      5�      �� 5�       �@J�Opacity$
 0.83Q�6(   �� 5�       � ��$ Times New Roman)�6+   5�      �� 5�   
   " ��6    .�5�      ��  � ��$ 14(�6)   �� 5�       � ��
 �����6+   �� 5�       � �� �	#99C197E16+   �� 5�       � ��$ 1q�60   �� 5�       � ��
 ���� 6,   �� 5�       � ��$ 15q�6(   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� #��z��6!   #��z��5�      #��z��5�      $ 5��5�      ._�5�      �� $��Center=�6/   $��Center=�6L   5�       5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6;   . �5�   "   ��  � �� �	#995A00A16?   5�      5�      5�      �� 5�       �@J�Opacity$
 0.83Q�6(   �� 5�       � ��$ Times New Roman)�6+   5�      �� 5�   
   " ��6'   .�5�      ��  � ��$ 14(�6)   �� 5�       � ��
 �����6+   �� 5�       � �� �#7E3DD16+   �� 5�       �@J�Opacity$	 0.9Q�6(   �� 5�       � ��$
 0,10q�6'   �� 5�       � �� �#21127A6,   �� 5�       � ��$ 1q�60   �� 5�       � ��$ Times New Roman)�6+   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� #��z��6!   #��z��5�      #��z��5�      $ 5��5�      ._�5�      �� $��Center=�6/   $��Center=�6L   5�       5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6;   . �5�   "   ��  � �� �#4003706?   5�      5�      5�      5�      �� 5�   
   " ��65   .�5�      ��  � ��$ 14(�6)   �� 5�       � ��
 �����6+   �� 5�       � �� �#7E3DD16+   �� 5�       �@J�Opacity$	 0.9Q�6(   �� 5�       � ��$
 0,10q�6'   �� 5�       � �� �#21127A6,   �� 5�       � ��$ 1q�60   �� 5�       � ��$ Times New Roman)�6+   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� #��z��6!   #��z��5�      #��z��5�      $ 5��5�      ._�5�      �� $��Center=�6/   $��Center=�6L   5�       5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6;   . �5�   "   ��  � �� �#4003706?   5�      5�      5�      5�      �� 5�   
   " ��6.   .�5�      ��  � ��$ 14(�6)   �� 5�       � ��
 �����6+   �� 5�       � �� �#7E3DD16+   �� 5�       �@J�Opacity$	 0.9Q�6(   �� 5�       � ��$
 0,10q�6'   �� 5�       � �� �#21127A6,   �� 5�       � ��$ 1q�60   �� 5�       � ��$ Times New Roman)�6+   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� #��z��6!   #��z��5�      #��z��5      $ 5��5     ._�5     �� $��Center=�6/   $��Center=�6L   5      5     P� 5     	 �@d�	IsChecked$

 TrueY�6;   . �5  "   ��  � �� �#4003706?   5     5	     5     5     �� 5  
   " ��6   .�5     ��  � �� �#DDC4FC6+   �� 5      �@J�Opacity$	 0.6Q�6(   �� 5      � ��$ Right=�64   �� 5      � ��$	 Top=�62   �� 5      � ��$ 0,422,549,0q�6'   �� 5      � ��$	 663��6&   �� 5      � ��$	 451��6'   5     �� 5  
   " �6(   .�5     ��  � ��$ 699,391,1086,554q�6'   �� 5      �@J�RenderTransformOrigin$ 
0.63,0.535&�66   �� 5      � ��$	 135��6&   �� 5      � �� �	#9021127A6+    5     P� 5      	 �@J�IsMouseOver
 . . �5!     ��  � �� �#5A00A163   5"     5$     �� 5'  
   " d�6*   .�5(     ��  � ��$ Times New Roman)�6+   �� 5)      � �� �#5A00A16+   �� 5*      � ��
 �����6+   �� 5+      � �� 5,     �� 5-     " d�6&   .R�5.     �� #��z��6!   ._�5/     �� $��Center=�6/   $��Center=�6L   50      51     P� 52     	 �@d�	IsChecked$

 TrueY�6;   . �53  "   ��  � ��
 �����6?   �� 54  "    � �� �#5A00A16?   55     P� 56     	 � {�
 .  . �57  "   ��  � ��
 �����6?   �� 58  "    � �� �#5A00A16?   59     5;     5=     5>     �� 5@  
   " d�6+   .�5A     ��  � ��$ Times New Roman)�6+   �� 5B      � ��
 �����6+   �� 5C      � �� �#5A00A16+   �� 5D      � �� 5E     �� 5F     " d�6&   .R�5G     �� #��z��6!   ._�5H     �� $��Center=�6/   $��Center=�6L   5I      5J     P� 5K     	 �@d�	IsChecked$

 TrueY�6;   . �5L  "   ��  � �� �#5A00A16?   �� 5M  "    � ��
 �����6?   5N     P� 5O     	 � {�
 .  . �5P  "   ��  � ��
 �   �6?   �� 5Q  "    � ��
 �  ��6?   5R     5T     5V     5W     �� 5\  
   " ��6+   .�5]     ��  � �� �#7E3DD16+   �� 5^      �@J�Opacity$
 0.58Q�6(   �� 5_      � ��$
 Left=�64   �� 5`      � ��$	 551��6'   �� 5a      � ��$ 350,422,0,0q�6'   �� 5b      � ��$	 Top=�62   �� 5c      � ��$	 308��6&   �� 5d      � �� 5e     �� 5f     " ��6   .�5g     ��  � ��
 ���� 67   �� 5h      � ��
 �����67    5i     P� 5j     	 �@��
IsSelected
 . . �5k  "   ��  � �� �#5A00A16?   5l     5n     5p     �� 5q      � �� 5r     �� 5s     " ��6&   .R�5t     �� #��z��6!   #��z��5u     #��z��5v     $ 20��5w     ._�5x     �� .��5y  "   �� 5z     5{     5|     5~     5     �� 5�  
   " ��6/   .�5�     ��  � ��
 �����6+   �� 5�      � �� �	#70F7C8FF6+   �� 5�      � ��$ 10q�6(   �� 5�      � ��$ 	15,40,0,0q�6'   �� 5�      � ��$	 177��6&   �� 5�      � ��$ Times New Roman)�6+   �� 5�      � �� 5�     �� 5�     " ��6&   .R�5�     �� #��z��6!   #��z��5�     #��z��5�     $ 20��5�     # z��5�     ._�5�     �� $��Center=�6/   $��Center=�6L   5�     5�     5�      5�     P� 5�     	 �@��
IsSelected
 . . �5�     ��  � �� �#5A00A163   5�     P� 5�     	 �@J�IsMouseOver
 . . �5�     ��  � �� �#5A00A163   5�     5�     �� 5�  
   " ��6-   .�5�     ��  � ��$ 20(�6)   �� 5�      � ��$
 Left=�64   �� 5�      � ��$ 80,-760,0,0q�6'   �� 5�      � ��$ Times New Roman)�6+   �� 5�      � �� �#5A00A16+   �� 5�      � ��
 �����6+   �� 5�      �@��Padding$ 	20,6,20,7q�6(   5�     �� 5�  
   " ��6*   .�5�     ��  � ��$ 	10,10,0,0q�6'   �� 5�      � ��$	 100��6&   �� 5�      �@J�Opacity$
 0.83Q�6(   �� 5�      � ��$ Times New Roman)�6+   �� 5�      � ��$ 14(�6)   �� 5�      � ��
 �����6+   �� 5�      � �� �#5A00A16+   �� 5�      � ��$ 1q�60   �� 5�      � ��
 ���� 6,   �� 5�      � ��$ 15q�6(   �� 5�      � �� 5�     �� 5�     " ��6&   .R�5�     �� #��z��6!   #��z��5�     #��z��5�     $ 5��5�     ._�5�     �� $��Center=�6/   $��Center=�6L   5�     5�     5�     5�     5�     .��5�     � $
��1920��6   $
��1080��6   .+�5�  
   r� -   5         r�Name$ mediaElement��5�      r� Source$; 5E:\C#\Mathematical cryptology\Images\sakura_music.mp4?�6-    r� Stretch$
 Fill=�5�  
    r� LoadedBehavior$
 Play=�5�  
    r� UnloadedBehavior$ Manual=�5�  
   $��	5,0,5,-72q�6$    r� Volume$ -2Q�67   �� 5�  
   -   5         ��Name$ 
lstMethods��5�     #��[ 6&   $��149,219,0,0q�6R   $	��465��6g   $	��323��6t   .@�5�     �� -   5        $��	15,80,0,0q�5�     $0��*Шифр частоколу [ Rail Fence ]��6.   #��[ 5�     5�     �� 5�     -   5        $+��%Шифр зсуву [ Caesar cipher ]��5�     #��[ 6A    J� RenderTransformOrigin$ 0.487,1.336&�5�  -   �� 5�     -   5        $4��.Шифр Віженера [ Vigenère cipher ]��5�     #��[ 6F   $	��214��5�  -   �� 5�     -   5        $/��)Шифр Кардано [ Cardan grille ]��5�     #��[ 6C   $	��181��5�  -   �� 5�     -   5        $/��)Афінний шифр [ Affine cipher ]��5�     #��[ 6C   $	��181��5�  -   �� 5�     $��Choose method��6   #��[ 6-   $ 0.619,-3.08&�6[   $��33��6   5�     �� 5�  
   ���	#99DDC4FC J� Opacity$	 0.7Q�6,   $��0,226,575,0q�6:   $	��664��6O   $	��451��6[   $ 0.728,0.511&�6h   �� 5�  
   -   5         ��Name$ englishCheckbox��5�     $	��139��6,   #��[ 68   $��1169,295,0,761q�6Y   $
��Left=�6q   $��English Alphabet��6�   6�   �� 5�  
   -	   5        $ ukrainianCheckbox��5�     $	��139��6.   #��[ 6:   $��1169,326,0,730q�6[   $
��Left=�6s   $��Ukrainian Alphabet��6�   $ -1.876,3.177&�6�   �� 5�  
   -
   5         ��Name$ 	textInput��5�     $
��Left=�6%   $��29��6@    �� TextWrapping$
 Wrap=�6L   $	��Top=�6a   $	��238��6y   $��893,306,0,0q�6�   .��6�   ��5�     �� 5�      �� Color$ #FFFFFF��6"    �� Opacity$	 0.4Q�62   .��5�  "   
Enter text5�     �� 5�  
   -   5        $ keyInput��5�     $
��Left=�6$   $	��Top=�6?   $	��176��6W   $��924,358,0,0q�6c   .��6�   ��5�     �� 5�     $ #FFFFFF��6"   $	 0.4Q�62   .��5�  "   Entered key5�     �� 5�  
   -   5        $ encryptCheckbox��5�     #��[ 6-   $	��136��6N   $��Encrypt��6Z   $��867,264,0,798q�6l   $
��Left=�6�   6�   �� 5�  
   -   5        $ decryptCheckbox��5�     #��[ 6-   $��Decrypt��6N   $��1013,264,0,798q�6`   $
��Left=�6x   $	��136��6�   6�   �� 5�  
   -   5        $ cryptoanalysisCheckbox��5�     $	��139��63   #��[ 6?   $��697,389,0,666q�6`   $
��Left=�6w   $��Cryptoanalysis��6�   6�   �� 5�  
   -   5        $ bruteForceCheckbox��5�     $	��139��6/   #��[ 6;   $��697,358,0,698q�6\   $
��Left=�6s   $��
Bruteforce��6�   6�   �� 5�  
   -   5        $ cryptoanalysisKFCheckbox��5�     $	��216��65   #��[ 6A   $��904,238,0,824q�6b   $
��Left=�6y   $'��!Kasiski-Friedman Cryptoanalysis  ��6�   $ 0.718,0.626&�6�   �� 5�  
   -   5        $ fileCheckbox��5�     #��[ 6H   $��Read from file��6i   $
��Left=�6�   $��697,325,0,0q�6�   $	��Top=�6�   $	��139��6�   $��25��6�   �� 5   
   -   5        $ textCheckbox��5      #��[ 6)   $��	Read text��6J   $
��Left=�6^   $��697,295,0,0q�6y   $	��Top=�6�   $	��139��6�   $��25��6�   6�   �� 5  
   -   5        $��Start��5     $
��Left=�6!   $	��Top=�6<   $	��118��6T   $��953,416,0,0q�6`   #��[ 6u   $��21��6�   6�   �� 5  
   -   5        $ resultOutput��5     $
��Left=�6(   $	��166��6C   $
 Wrap=�6P   $�� ��6d   $	��294��6l   $��	865,0,0,0q�6x   �� 5  
   $��/deussdelli.png��6   $
��None=�5     $��-282,362,282,13q�6   �� 5	  
   $6��0E:\C#\Mathematical cryptology\Images\sakura2.png��6   $
��None=�6J   $	��NaN��6Y   $	��NaN��6f   $��117,-172,729,619q�6r   $ 0.504,0.616&�6�   5
     5     