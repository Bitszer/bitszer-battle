//
//  ETViewController.m
//  ETPluginTemp
//
//  Created by Rana Ijaz on 2/29/16.
//  Copyright © 2016 Rana Ijaz. All rights reserved.
//

#import "ETViewController.h"
//#import "ProductName-Swift.h"
#import <BitszerFramework7_3/BitszerFramework7_3-Swift.h>

static ETViewController *singleton;

@interface ETViewController ()

@end

@implementation ETViewController

+(instancetype)sharedInstance{
    if (!singleton) {
        singleton = [[ETViewController alloc] init];
    }
    return singleton;
}


- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    
}

-(void)postDataToUnity:(NSNotification*)notification{
    NSLog(@"postDataToUnity called: %@", notification.object);
    const char *str = [notification.object UTF8String];
    _getUserProfileResponse(str);
    //[[self.rootViewController presentingViewController] dismissViewControllerAnimated:YES completion:nil];
    
    [UIApplication sharedApplication].keyWindow.rootViewController = self.rootViewController;
    //[[self.rootViewController presentingViewController] dismissViewControllerAnimated:YES completion:nil];
}

-(void)loadNativeVC:(NSString *)requiredItemsStr updateProducts:(NSString *)updateProducts gameID:(NSString*)gameID{
    NSLog(@"loadNativeVC called");
    NSLog(@"loadNativeVC called with gameID: %@", gameID);
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(postDataToUnity:) name:@"postDataToUnityNotification" object:nil];
    //SwiftControllerVC *swiftCtr = nil;
    //LoginVC *loginVC = [[LoginVC alloc] init];
    
    
    NSBundle *pluginBundle = [NSBundle bundleForClass:LoginVC.class]; //[NSBundle bundleWithIdentifier:@"com.geniteam.BitszerFramework7-3"];
    UIStoryboard *sb = [UIStoryboard storyboardWithName:@"Login" bundle:pluginBundle];
    ETViewController *vc = [sb instantiateViewControllerWithIdentifier:@"ViewControllerID"];
    
    [[NSUserDefaults standardUserDefaults] setValue:requiredItemsStr forKey:@"RequiredItemsStr"];
    
    [[NSUserDefaults standardUserDefaults] setValue:updateProducts forKey:@"updateProductsStr"];
    [[NSUserDefaults standardUserDefaults] setValue:@"" forKey:@"AllProductsStr"];
    [[NSUserDefaults standardUserDefaults] setValue:gameID forKey:@"gameId"];
    
    //vc.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;
    //[self presentViewController:vc animated:YES completion:NULL];
    //ViewController *vc = [[ViewController alloc] initWithNibName:@"ViewController" bundle:nil];
    
    NSLog(@"GetTopVCUtility now");
    self.rootViewController = [GetTopVCUtility topViewController];
    if (self.rootViewController) {
        NSLog(@"rootViewController found");
        [self.rootViewController presentViewController:vc animated:YES completion:nil];
    }
    else{
        NSLog(@"rootViewController Not found");
    }
    
    //NSLog(@"Post notification now");
    //[[NSNotificationCenter defaultCenter] postNotificationName:@"postDataToUnityNotification" object:@"Hello Data 1"];
}

-(void)loadNativeVCToUpdate:(NSString *)updateProducts gameID:(NSString*)gameID{
    NSLog(@"loadNativeVC called");
    NSLog(@"loadNativeVC called with gameID: %@", gameID);
    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(postDataToUnity:) name:@"postDataToUnityNotification" object:nil];
    
    
    NSBundle *pluginBundle = [NSBundle bundleForClass:LoginVC.class]; //[NSBundle bundleWithIdentifier:@"com.geniteam.BitszerFramework7-3"];
    UIStoryboard *sb = [UIStoryboard storyboardWithName:@"Login" bundle:pluginBundle];
    ETViewController *vc = [sb instantiateViewControllerWithIdentifier:@"ViewControllerID"];
    
    [[NSUserDefaults standardUserDefaults] setValue:updateProducts forKey:@"updateProductsStr"];
    [[NSUserDefaults standardUserDefaults] setValue:@"" forKey:@"RequiredItemsStr"];
    [[NSUserDefaults standardUserDefaults] setValue:@"" forKey:@"AllProductsStr"];
    [[NSUserDefaults standardUserDefaults] setValue:gameID forKey:@"gameId"];
    
    NSLog(@"GetTopVCUtility now");
    self.rootViewController = [GetTopVCUtility topViewController];
    if (self.rootViewController) {
        NSLog(@"rootViewController found");
        [self.rootViewController presentViewController:vc animated:YES completion:nil];
    }
    else{
        NSLog(@"rootViewController Not found");
    }
    
    //NSLog(@"Post notification now");
    //[[NSNotificationCenter defaultCenter] postNotificationName:@"postDataToUnityNotification" object:@"Hello Data 1"];
}



- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
