//
//  CWrapper.h
//  MusicLibPlugin
//
//  Created by AppTech Machines on 10/16/14.
//  Copyright (c) 2014 AppTech Machines. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ETViewController.h"

@interface CWrapper : NSObject

@end


#ifdef __cplusplus
extern "C" {
#endif
    void _loadNativeVC(const char* string, const char* string2, const char* gameID);
    void _loadNativeVCToUpdate(const char* string, const char* gameID);
    void _getUserProfileResponse(const char *response);
    
    //void _openMusicLibrary();
    //void _getMusicFilePath(const char *filePath);
    
    //void _playMusic(const char *fileURL);
    
#ifdef __cplusplus
}
#endif

