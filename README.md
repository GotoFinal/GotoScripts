# My script collection

This is small collection of Unity scripts I've made for vrchat to solve some small issues I had when creating avatars.  

※このドキュメント内の日本語翻訳はすべて自動翻訳です。  
これは、VRChat用のアバターを作成する際に直面したいくつかの小さな問題を解決するために作成した、Unityスクリプトの小さなコレクションです。

## Crunch Begone
Simple script that automatically disables crunch compression on all assets. 
Note that it makes using crunch basically impossible, but you should not be using crunch for avatar projects anyways.
Crunch does not improve vram, and difference in download size is very small, while the quality is much worse and can cause extra lag spikes when loading the avatar.

全てのアセットに対してクランチ圧縮を自動的に無効化するシンプルなスクリプトです。
このスクリプトを使うとクランチの使用はほぼ不可能になりますが、そもそもアバタープロジェクトではクランチを使うべきではありません。
クランチはVRAMの節約にはならず、ダウンロードサイズの差もごくわずかです。その一方で画質が大幅に劣化し、アバターの読み込み時にラグの原因になることがあります。

## Optimizer Control
Automatically skips popular optimizer scripts like anatawa12/AvatarOptimizer and d4rkAvatarOptimizer when entering play mode, but still applying everything when uploading the avatar.  
You can also change the default mode or make single-time build with optimziers enabled/disabled.  
プレイモードに入るときに、anatawa12/AvatarOptimizer や d4rkAvatarOptimizer などの一般的な最適化スクリプトを自動的にスキップしますが、アバターのアップロード時にはすべて適用されます。
デフォルトモードを変更したり、一度だけ最適化を有効／無効にしてビルドすることも可能です。  
![image](https://github.com/user-attachments/assets/e5ca161b-e283-4044-b153-7bce84864217)  

Additionaly it has extra component to allow controlling any other tool manually, any object or component on this list will be removed at the very beginning of a build.  
You can include here any tools that take long time to process that you don't need when just quickly testing avatar in Unity.  
さらに、任意のツールを手動で制御できるコンポーネントも含まれています。このリストにあるオブジェクトやコンポーネントはビルドの最初に削除されます。
Unityでアバターを素早くテストしたいときに、処理時間の長いツールをここに追加することができます。  
![image](https://github.com/user-attachments/assets/e21ab678-e251-49aa-825e-f59cfe8bcb2c)

This is example of my avatar when entering play with with all optimizations enabled:  
すべての最適化を有効にした状態でプレイモードに入ったアバターの例：   
![image](https://github.com/user-attachments/assets/b46a4062-60b6-4a79-bc72-0dfe99e1d2cb)  
And with optimziers disabled + light control disabled:   
最適化とライト制御を無効にした状態の例：   
![image](https://github.com/user-attachments/assets/98a2df71-9836-4be3-9a69-dee4fbdb25d6)

## Object remover
Allows for removal of some objects or singular components on build while also hiding them from view in editor, this can either help you optimise the avatar with easy way to undo it or save a lot of build time by removing objects before other tools will try to process them.  
This has small advantage over just removing the object that its easier to re-add it on prefab variants.  
ビルド時に特定のオブジェクトやコンポーネントを削除し、エディタ内でも非表示にできるツールです。
他のツールが処理を始める前に対象を削除できるので、アバターの最適化やビルド時間の短縮に役立ちます。
また、オブジェクトを削除するよりもプレハブバリアントで再追加しやすい利点もあります。   
![image](https://github.com/user-attachments/assets/5b2f18d9-eaf3-4d3e-9899-553337654b60)  
The objects are also by default hidden from hierarchy view, just as if they would be removed:  
削除されたように、オブジェクトはデフォルトでヒエラルキーにも表示されません：   
![image](https://github.com/user-attachments/assets/556ccf67-d45d-4e16-945b-fd383d854d0d) ![image](https://github.com/user-attachments/assets/70a51837-76af-480a-9c72-a9e82223f811)

## PhysBones Scene Dirty Patch
Small patch that patches vrchat PhysBones to not dirty whole scene when bone values are edited, its not needed and causes some other tools to re-scan whole project causing in-editor lag.  
VRChatのPhysBonesの値を編集した際に、シーン全体が変更済み（Dirty）としてマークされるのを防ぐ小さなパッチです。
これは本来不要であり、他のツールがプロジェクト全体を再スキャンしてエディタ内でのラグの原因になります。   

## Play mode benchmark
The simple tool I use to measure how long it takes to enter play mode, example of log visible in section above about Optimizer Control.  
プレイモードに入るまでの時間を測定するためのシンプルなツールです。 
ログの例は上記の「Optimizer Control」セクションに表示されています。  

