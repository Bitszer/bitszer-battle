//
//  ETViewController.h
//  ETPluginTemp
//
//  Created by Rana Ijaz on 2/29/16.
//  Copyright © 2016 Rana Ijaz. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "GetTopVCUtility.h"
#import "CWrapper.h"

@interface ETViewController : UIViewController


@property(nonatomic, retain) UIViewController *rootViewController;


+(instancetype)sharedInstance;
-(void)loadNativeVC:(NSString *)requiredItemsStr updateProducts:(NSString *)updateProducts gameID:(NSString*)gameID;
-(void)loadNativeVCToUpdate:(NSString *)requiredItemsStr gameID:(NSString*)gameID;

@end

