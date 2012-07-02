#include "stdafx.h"
#include "MonoApplication.h"

#include "PathUtil.h"

CMonoApplication::CMonoApplication()
{
	mono_set_dirs(PathUtil::GetLibDirectory().c_str(), PathUtil::GetConfigDirectory().c_str());

	// Required for mdb's to load for detailed stack traces etc.
	mono_debug_init(MONO_DEBUG_FORMAT_MONO);

	m_pRootDomain = mono_jit_init_version("MonoApplication", "v4.0.30319");

	MonoAssembly *pMonoAssembly = mono_domain_assembly_open(mono_domain_get(), PathUtil::GetBinDirectory().append("ClassLibrary.dll").c_str());
	m_pClassLibraryImage = mono_assembly_get_image(pMonoAssembly);

	m_pClassLibraryManagerClass = mono_class_from_name(m_pClassLibraryImage, "ClassLibraryNamespace", "ClassLibraryManager");

	m_pClassLibraryManager = mono_object_new(mono_domain_get(), m_pClassLibraryManagerClass);
	mono_runtime_object_init(m_pClassLibraryManager);
}

CMonoApplication::~CMonoApplication()
{
}

bool CMonoApplication::Run()
{
	return true;
}