/*
 * Created by SharpDevelop.
 * User: Kallan Sipple
 * Date: 17/06/2019
 * Time: 23:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;  
using System.Collections.Generic;  
using System.ComponentModel;  
using System.Data;  
using System.Drawing;  
using System.Linq;  
using System.Text;  
using System.Windows.Forms;  
using System.IO;

namespace Blog_Editor
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public string final_text;  // holds the blog post entry
		public string page_link; // holds the page link for the new page
		string page_path; // holds the actual path for the blog page creation
		public string image_path; // holds the chosen image file location
		public string directory = @"C:\Users\HP\Desktop\MySite\Dani"; // Directory where project is being held
		public string image_code; // html code for adding image tag + link
		public string image_name; // name of the image file, without path
		public string video_link; // stores the video link added by user;
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();		
            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
            		
		}
		
		private void button1_Click(object sender, EventArgs e){
			
			// All the following code in this section is related to the Blog's "page entry", and not page creation. 
			
			string base_template = File.ReadAllText(@"C:\Users\HP\Desktop\template.txt");
				
				base_template = base_template.Replace("<!--TITULO-->", title_Box.Text);
				base_template = base_template.Replace("<!--DATA-->", date_Box.Text);
				base_template = base_template.Replace("<!--CATEGORIA-->", category_Box.Text);
				base_template = base_template.Replace("<!--AUTOR-->", author_Box.Text);
				base_template = base_template.Replace("<!--PARAGRAFO_1-->", textBox1.Text);
				
				page_link= title_Box.Text +"-"+ date_Box.Text;   //
				page_link= page_link.Replace(" ", string.Empty); //
				page_link= page_link.Replace(".", string.Empty); // All this section creates a uniques link for the blog post page; 
				page_link= page_link.Replace(",", string.Empty); //
				page_link=page_link.ToLower();					 //
				
				base_template = base_template.Replace("<!--LINK-->", page_link); // Adds the link created just before to the "base file";
			
				
				string path_landingpage= directory + "\\blog.html";
				string open = File.ReadAllText(path_landingpage);  // Reads and copies content of Blog's "entry page";

				open = open.Replace("<!-- REPLACE -->", base_template); // Then, replaces the indicated tag with the "base file" content;
				
													
													// CONTENT COMPLETE! //
				
				final_text=open;								   // Sends complete content ("entry") to globla variable (to be used afterwards). 
				page_path= directory + "\\" + page_link + ".html"; // Sends link of this entry for file to be created; 
				
				checkBox3.Checked=true;  // To show to user that file has progressed ok. 
					
		}
		
		private void button2_Click(object sender, EventArgs e)   
        {  
				// This code concerns the INSERTION of the entry elaborated above onto the Blog's "entry page".
			
                string path_landingpage= directory + "\\blog.html"; // First, find the "entry page" location and name.
                File.WriteAllText(path_landingpage, final_text);    // Then, replace everything with new version with entry. 
                
                
                // This code concerns the CREATION of its individual page.
                 
                string base_template = File.ReadAllText(@"C:\Users\HP\Desktop\template_2.txt");  //Contains the basic code for a standalone page;
	          
	             base_template= base_template.Replace("<!--TITULO-->", title_Box.Text);       //
	             base_template= base_template.Replace("<!--DATA-->", date_Box.Text);		  //
	             base_template= base_template.Replace("<!--CATEGORIA-->", category_Box.Text); // All this section replaces the given tags with actual content;
	             base_template= base_template.Replace("<!--AUTOR-->", author_Box.Text);		  //
				 base_template= base_template.Replace("<!--PARAGRAFO-->", textBox1.Text);	  //
				 
			    if (!String.IsNullOrEmpty(textBox4.Text)){	// If Image path is not empty, 
				 	
	             	image_name= Path.GetFileName(image_path); // Send the given path to Global variable;
	             	image_code= "<img src=\"" + directory + "\\" + image_name + "\">"; // Create .html code to add to page;
	             	string image_code2= "<img src=\"" + image_name + "\">";
	             	base_template= base_template.Replace("<!--IMAGEM-->", image_code2); // Replace given tag (in the copied basic code) with the code created above.
	             }
				 
				 if (!String.IsNullOrEmpty(video_link)){
				 	
				 	string video_code= "<iframe style=\"height:325px;\" width=\"560\"" + "" + "src=\"" + video_link + "\"></iframe>"; // Iframe code with youtube video link.
				 	base_template= base_template.Replace("<!--IMAGEM-->", video_code); // Replaces the entry "IMAGE" with above code in template;
				 	MessageBox.Show(video_code); 
				 }
				 
				 string paragraph2= "<p>" + textBox2.Text + "</p>"; // Paragraph 2 .html code (need to be outside to be
				 													// accessible in 2nd "if statement"). 
				
				 if (checkBox1.CheckState == CheckState.Checked){ 	// If checkbox checked,
				 	base_template= base_template.Replace("<!--2ndPARAGRAPH-->", paragraph2); // Replace tag (in the copied basic code) with content.
				 }
				 													
				 if (checkBox1.CheckState == CheckState.Checked && checkBox2.CheckState == CheckState.Checked){ // If BOTH checked,
				 	string paragraph3= "<p>" + textBox3.Text + "</p>";							//Create Paragraph 3 .html code;
				 	base_template= base_template.Replace("<!--2ndPARAGRAPH-->", paragraph2);	// Replace tag (in the copied basic code) with content;
				 	base_template= base_template.Replace("<!--3rdPARAGRAPH-->", paragraph3);	// Replace tag (in the copied basic code) with content.
				 }
				 													
				 else{
				 	Console.WriteLine("No extra paragraphs checked!"); // If NO checkboxes are active, diplay on Console. 
				 }
				 
													// DATABASE CODE //
													
				//For keeping track of all pages created and,
				// retroactively add working "previous" and "next" links
				// to each individual page (current and already created).
				
				if( new FileInfo( @"C:\Users\HP\Desktop\logger.txt" ).Length == 0 )	// File for keeping track of created pages;
				{																	// If it is empty (brand new or erased),
					
//					using (StreamWriter swriter = new StreamWriter(@"C:\Users\HP\Desktop\logger.txt", true))
//                	{
//                		swriter.WriteLine(page_link); // Write the page's link to it. 
//                	}
					File.WriteAllText(@"C:\Users\HP\Desktop\logger.txt", page_link);
					base_template= base_template.Replace("<!--ARTICLE-LINK_Pr-->", "#");
				}
				
				else if	( new FileInfo( @"C:\Users\HP\Desktop\logger.txt" ).Length != 0 )// If it is not empty, 
				{
					string link_file= File.ReadAllText(@"C:\Users\HP\Desktop\logger.txt"); // Copy last blog post's link contained in file 
																					   // to be used in the "Previous" link;
				
					//Delete entry, Add new entry with new page's link.
					File.WriteAllText(@"C:\Users\HP\Desktop\logger.txt", page_link);
					
					// Add copied link to blog post file
					base_template= base_template.Replace("<!--ARTICLE-LINK_Pr-->", link_file +".html");
					//base_template= base_template.Replace("<!--ARTICLE-LINK_Nx-->", "#");
					LinkTagReplacer(link_file);
				}
								 
                 
//                using (StreamWriter swriter = new StreamWriter(page_path, true))
//                {
//                	swriter.WriteLine(base_template);
//                }
                
                File.WriteAllText(page_path, base_template);
				 
              	// move the choosen image to the correct directory...
              	if (!String.IsNullOrEmpty(textBox4.Text)){
              		if (File.Exists(directory + "\\" + image_name))
					{
              			Console.WriteLine("Image already exists in directory");
              		}
              		else{
              			string c_image_code= directory + "\\" + image_name;
	              		File.Copy(image_path, c_image_code);
              		}
              	}
              	
              	
              	 MessageBox.Show("Arquivos criados e atualizados com sucesso!"); 
              	 checkBox3.Checked=false;
              	 textBox4.Text="";
            //}  
        }
		void Button3Click(object sender, EventArgs e)
		{
			openFileDialog2.ShowDialog();
			string name= openFileDialog2.FileName;
			textBox4.Text=name;

			image_path= name;
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(String.IsNullOrEmpty(textBox5.Text)){
				MessageBox.Show("Inserir link!");
			}
			else{
				video_link= textBox5.Text; 
				MessageBox.Show(video_link);
			}
		}
		
				
		void LinkTagReplacer(string previous_page) //contains previous page name, but without ".html"
		{
			
			string previous_full= previous_page + ".html"; // adds .html extension, so we can load the previous page file
			if (File.Exists(directory + "\\" + previous_full))
			{
			
				string read_file= File.ReadAllText(directory + "\\" + previous_full); // reads the whole page;
			
				read_file= read_file.Replace("<!--ARTICLE-LINK_Nx-->", "<li class=\"next\"><a rel=\"next\" href=\"" + page_link + ".html" + "\"><strong>Next Article</strong> </a></li>");
				
				File.WriteAllText(directory + "\\" + previous_full, read_file); 
				
//				using (StreamWriter swriter = new StreamWriter(directory + "\\" + previous_full, true))
//                {
//                	swriter.WriteLine(read_file);
//                }
				MessageBox.Show("link written...");
			}
			else
			{
				MessageBox.Show("dead link!");
			}
			
			
		}

	}
}
