#ifndef __MONO_APPLICATION_H__
#define __MONO_APPLICATION_H__

#include <mono/mini/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/mono-debug.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/appdomain.h>
#include <mono/metadata/object.h>
#include <mono/metadata/threads.h>
#include <mono/metadata/environment.h>
#include <mono/metadata/mono-gc.h>

class CMonoApplication
{
public:
	CMonoApplication();
	~CMonoApplication();

	bool Run();

private:
	MonoDomain *m_pRootDomain;
	MonoImage *m_pClassLibraryImage;

	MonoClass *m_pClassLibraryManagerClass;
	MonoObject *m_pClassLibraryManager;
};

#endif //__MONO_APPLICATION_H__