//
//  CWrapper.m
//  MusicLibPlugin
//
//  Created by AppTech Machines on 10/16/14.
//  Copyright (c) 2014 AppTech Machines. All rights reserved.
//

#import "CWrapper.h"
extern void UnitySendMessage(const char* , const char* , const char*);

@implementation CWrapper


@end


NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

char* CreateCString (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

void _loadNativeVC(const char* string, const char* string2, const char* gameID){
    NSLog(@"_loadNativeVC called");
    NSLog(@"Dictionary is: %s", string);
    NSLog(@"Dictionary for update is: %s", string2);
    NSLog(@"loadNativeVC called with gameID: %s", gameID);
    [[ETViewController sharedInstance] loadNativeVC:[NSString stringWithUTF8String:string] updateProducts:[NSString stringWithUTF8String:string2] gameID:[NSString stringWithUTF8String:gameID]];
}

void _loadNativeVCToUpdate(const char* string, const char* gameID){
    NSLog(@"_loadNativeVCToUpdate called");
    NSLog(@"Dictionary is: %s", string);
    NSLog(@"loadNativeVC called with gameID: %s", gameID);
    [[ETViewController sharedInstance] loadNativeVCToUpdate:[NSString stringWithUTF8String:string] gameID:[NSString stringWithUTF8String:gameID]];
}

void _getUserProfileResponse(const char *response)
{
    NSLog(@"sending call back from CWrapper");
    UnitySendMessage("AuctionHouse", "updatedProducts", CreateCString(response));
    NSLog(@"sent call back from CWrapper");
}

/*
void _openMusicLibrary(){
    NSLog(@"CWrapper method _openMusicLibrary called");
    //MusicLibViewController *musicLibVC = [[MusicLibViewController alloc] init];
    [[MusicLibViewController sharedInstance] openMusicLibrary];
}

void _getMusicFilePath(const char *filePath)
{
    NSLog(@"sending call back from CWrapper");
    UnitySendMessage("PhotoSelectionManager", "getMusicFilePath", CreateCString(filePath));
}

void _playMusic(const char *fileURL){
    NSLog(@"CWrapper method _playMusic called");
    [[MusicLibViewController sharedInstance] playMusicOnURL:[NSString stringWithUTF8String:fileURL]];
}
*/






