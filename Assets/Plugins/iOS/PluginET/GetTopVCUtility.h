//
//  PluginUtility.h
//  facebookPlugin
//
//  Created by Nawaz on 8/26/13.
//  Copyright (c) 2013 GeniTeam. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface GetTopVCUtility : NSObject

+(UIViewController *)topViewController;
+(UIViewController *)topViewController:(UIViewController *)rootViewController;
@end
