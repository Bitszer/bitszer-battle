//
//  PluginUtility.m
//  facebookPlugin
//
//  Created by Nawaz on 8/26/13.
//  Copyright (c) 2013 GeniTeam. All rights reserved.
//

#import "GetTopVCUtility.h"

@implementation GetTopVCUtility

+(UIViewController *)topViewController{
    return [self topViewController:[UIApplication sharedApplication].keyWindow.rootViewController];
}
+(UIViewController *)topViewController:(UIViewController *)rootViewController
{
    if (rootViewController.presentedViewController == nil) {
        NSLog(@"root view persented view controller == nil");
        NSLog(@"%@",rootViewController);
        NSLog(@"top view x=%f,y=%f,w=%f,h=%f",rootViewController.view.frame.origin.x,rootViewController.view.frame.origin.y,rootViewController.view.frame.size.width,rootViewController.view.frame.size.height);
        return rootViewController;
    }
    
    if ([rootViewController.presentedViewController isMemberOfClass:[UINavigationController class]]) {
        UINavigationController *navigationController = (UINavigationController *)rootViewController.presentedViewController;
        UIViewController *lastViewController = [[navigationController viewControllers] lastObject];
        return [self topViewController:lastViewController];
    }
    
    UIViewController *presentedViewController = (UIViewController *)rootViewController.presentedViewController;
    return [self topViewController:presentedViewController];
}


@end
