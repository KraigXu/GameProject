using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum ENUM_OBJECT_TYPE                               
//{
//    OBJECT_UNKNOW = 0x00,                                  
//    OBJECT_ITEM = 0x01,                                    
//    OBJECT_MAIL = 0x02,                                    
//};

//public enum ENUM_OBJECT_STATE                              
//{
//    OBJECT_UNKNOW_STATE = 0x00,                            
//    OBJECT_DISPLAY_STATE = 0x01,                           
//    OBJECT_INVALID_STATE = 0x02,                           
//};
//public enum ENUM_ITEM_ATTRIBUTE : int
//{
//    ITEM_ATTRIBUTE_NONE = -1,
//    ITEM_ATTRIBUTE_MIN_ATTACK = 0x00,                      
//    ITEM_ATTRIBUTE_MAX_ATTACK = 0x01,                      
//    ITEM_ATTRIBUTE_PHYSICS_DEFENCE = 0x02,                 
//    ITEM_ATTRIBUTE_MAGIC_DEFENCE = 0x03,                   
//    ITEM_ATTRIBUTE_LIFE = 0x04,                            
//    ITEM_ATTRIBUTE_MAGIC = 0x05,                           
//    ITEM_ATTRIBUTE_POWER = 0x06,                           
//    ITEM_ATTRIBUTE_CRIT = 0x07,                            
//    ITEM_ATTRIBUTE_ATTACKSPEED = 0x08,                     
//    ITEM_ATTRIBUTE_HIT = 0x09,                             
//    ITEM_ATTRIBUTE_DODGE = 0x0A,                           
//    ITEM_ATTRIBUTE_RECOVER_LIFE = 0x1F,                    
//    ITEM_ATTRIBUTE_RECOVER_MAGIC = 0x0C,                   
//    ITEM_ATTRIBUTE_SKILL_ATTACK = 0x0D,                    
//    ITEM_ATTRIBUTE_SKILL_P_DEFENCE = 0x0E,                 
//    ITEM_ATTRIBUTE_SKILL_M_DEFENCE = 0x0F,                 
//    ITEM_ATTRIBUTE_SKILL_CRIT = 0x10,                      
//    ITEM_ATTRIBUTE_SKILL_HIT = 0x11,                       
//    ITEM_ATTRIBUTE_SKILL_A_SPEED = 0x12,                   
//    ITEM_ATTRIBUTE_SKILL_DODGE = 0x13,                     
//    ITEM_ATTRIBUTE_POSITION = 0x14,                        
//    ITEM_ATTRIBUTE_CLASS = 0x15,                           
//    ITEM_ATTRIBUTE_PHOTOID = 0x16,                         
//    ITEM_ATTRIBUTE_BASEID = 0x17,                          
//    ITEM_ATTRIBUTE_JEWEL_COUNT = 0x18,                  
//    ITEM_ATTRIBUTE_JEWEL_1 = 0x19,                         
//    ITEM_ATTRIBUTE_JEWEL_2 = 0x1A,                         
//    ITEM_ATTRIBUTE_JEWEL_3 = 0x1B,                         
//    ITEM_ATTRIBUTE_JEWEL_4 = 0x1C,                         
//    ITEM_ATTRIBUTE_JEWEL_5 = 0x1D,                         
//    ITEM_ATTRIBUTE_VERSION = 0x1E,                         
//    ITEM_ATTRIBUTE_USE_LEVEL = 0x1F,                       
//    ITEM_ATTRIBUTE_LEVEL = 0x20,                           
//    ITEM_ATTRIBUTE_SYNTHETIC_COUNT = 0x21,                 
//    ITEM_ATTRIBUTE_SYN_BASEID_1 = 0x22,                    
//    ITEM_ATTRIBUTE_SYN_BASEID_2 = 0x23,                    
//    ITEM_ATTRIBUTE_SYN_BASEID_3 = 0x24,                    
//    ITEM_ATTRIBUTE_SYN_BASEID_4 = 0x25,                    
//    ITEM_ATTRIBUTE_SYN_BASEID_5 = 0x26,                    
//    ITEM_ATTRIBUTE_SYN_BID_1_COUNT = 0x27,                 
//    ITEM_ATTRIBUTE_SYN_BID_2_COUNT = 0x28,                 
//    ITEM_ATTRIBUTE_SYN_BID_3_COUNT = 0x29,                 
//    ITEM_ATTRIBUTE_SYN_BID_4_COUNT = 0x2A,                 
//    ITEM_ATTRIBUTE_SYN_BID_5_COUNT = 0x2B,                 
//    ITEM_ATTRIBUTE_SYN_NEWID_COUNT = 0x2C,                 
//    ITEM_ATTRIBUTE_SYN_NEWID_1 = 0x2D,                     
//    ITEM_ATTRIBUTE_SYN_NEWID_2 = 0x2E,                     
//    ITEM_ATTRIBUTE_SYN_NEWID_3 = 0x2F,                     
//    ITEM_ATTRIBUTE_SYN_NEWID_C_1 = 0x30,                   
//    ITEM_ATTRIBUTE_SYN_NEWID_C_2 = 0x31,                   
//    ITEM_ATTRIBUTE_SYN_NEWID_C_3 = 0x32,                   
//};

//public enum ENUM_ITEM_TEXT : int
//{
//    ITEM_TEXT_NAME = 0,                                    
//    ITEM_TEXT_EXPAIN = 1,                                  
//};

//public enum ENUM_ITEM_CLASS : int
//{
//    ITEM_CLASS_SKILL_BOOK = 0x00,                          
//    ITEM_CLASS_BOX = 0x01,                                 
//    ITEM_CLASS_EQUIPMENT = 0x02,                           
//    ITEM_CLASS_RESOURCE = 0x03,                            
//    ITEM_CLASS_JEWEL = 0x04,                               
//    ITEM_CLASS_RUNE = 0x05,                                
//    ITEM_CLASS_STONE = 0x06,                               
//    ITEM_CLASS_TASK = 0x07,                           
//    ITEM_CLASS_DRAW = 0x08,                                
//    ITEM_CLASS_WEAPON = 0x09,                              
//};

public enum ENUM_EFFECT_CLASS : int
{
    EFFECT_CLASS_ = 0x00,
    EFFECT_CLASS_BOX = 0x01,
    EFFECT_CLASS_EQUIPMENT = 0x02,
    EFFECT_CLASS_RESOURCE = 0x03,
    EFFECT_CLASS_JEWEL = 0x04,
    EFFECT_CLASS_RUNE = 0x05,
    EFFECT_CLASS_STONE = 0x06,
    EFFECT_CLASS_TASK = 0x07,
    EFFECT_CLASS_DRAW = 0x08,
    EFFECT_CLASS_WEAPON = 0x09,
}

public class TechniquesBaseObject  {


}
